﻿using System;
using System.IO;
using YARG.Core.Engine;
using YARG.Core.Engine.Drums;
using YARG.Core.Engine.Guitar;
using YARG.Core.Engine.Logging;
using YARG.Core.Engine.ProKeys;
using YARG.Core.Engine.Vocals;
using YARG.Core.Game;
using YARG.Core.Input;
using YARG.Core.IO;
using YARG.Core.Song;

namespace YARG.Core.Replays
{
    public static class ReplaySerializer
    {
        #region Replay

        public static void SerializeReplay(BinaryWriter writer, ReplayNew replayNew)
        {
            SerializeHeader(writer, replayNew.Header);
            SerializeMetadata(writer, replayNew.Metadata);
            SerializePresetContainer(writer, replayNew.PresetContainer);

            writer.Write(replayNew.PlayerCount);
            foreach (var playerName in replayNew.PlayerNames)
            {
                writer.Write(playerName);
            }

            foreach (var frame in replayNew.Frames)
            {
                SerializeFrame(writer, frame);
            }
        }

        public static ReplayNew? DeserializeReplay(BinaryReader reader, int version = 0)
        {
            var replay = new ReplayNew();

            replay.Header = DeserializeHeader(reader, version);
            replay.Metadata = DeserializeMetadata(reader, version);
            replay.PresetContainer = DeserializePresetContainer(reader, version);

            int playerCount = reader.ReadInt32();
            var playerNames = new string[playerCount];
            for (int i = 0; i < playerCount; i++)
            {
                playerNames[i] = reader.ReadString();
            }

            replay.Frames = new ReplayFrame[playerCount];

            for (int i = 0; i < playerCount; i++)
            {
                replay.Frames[i] = DeserializeFrame(reader, version);
            }

            return replay;
        }

        #endregion

        #region Header

        private static void SerializeHeader(BinaryWriter writer, ReplayHeader header)
        {
            header.Magic.Serialize(writer);

            writer.Write(header.ReplayVersion);
            writer.Write(header.EngineVersion);

            header.ReplayChecksum.Serialize(writer);
        }

        private static ReplayHeader DeserializeHeader(BinaryReader reader, int version = 0)
        {
            var header = new ReplayHeader();

            header.Magic = EightCC.Read(reader.BaseStream);
            header.ReplayVersion = reader.ReadInt32();
            header.EngineVersion = reader.ReadInt32();
            header.ReplayChecksum = HashWrapper.Deserialize(reader);

            return header;
        }

        #endregion

        #region Metadata

        private static void SerializeMetadata(BinaryWriter writer, ReplayMetadata metadata)
        {
            writer.Write(metadata.SongName);
            writer.Write(metadata.ArtistName);
            writer.Write(metadata.CharterName);
            writer.Write(metadata.BandScore);
            writer.Write((int) metadata.BandStars);
            writer.Write(metadata.ReplayLength);
            writer.Write(metadata.Date.ToBinary());
            metadata.SongChecksum.Serialize(writer);
        }

        private static ReplayMetadata DeserializeMetadata(BinaryReader reader, int version = 0)
        {
            var metadata = new ReplayMetadata();

            metadata.SongName = reader.ReadString();
            metadata.ArtistName = reader.ReadString();
            metadata.CharterName = reader.ReadString();
            metadata.BandScore = reader.ReadInt32();
            metadata.BandStars = (StarAmount) reader.ReadByte();
            metadata.ReplayLength = reader.ReadDouble();
            metadata.Date = DateTime.FromBinary(reader.ReadInt64());
            metadata.SongChecksum = HashWrapper.Deserialize(reader);

            return metadata;
        }

        #endregion

        #region Preset Container

        private static void SerializePresetContainer(BinaryWriter writer, ReplayPresetContainer presetContainer)
        {
            presetContainer.Serialize(writer);
        }

        private static ReplayPresetContainer DeserializePresetContainer(BinaryReader reader, int version = 0)
        {
            var presetContainer = new ReplayPresetContainer();
            presetContainer.Deserialize(reader, version);
            return presetContainer;
        }

        #endregion

        #region Frame

        private static void SerializeFrame(BinaryWriter writer, ReplayFrame frame)
        {
            SerializePlayerInfo(writer, frame.PlayerInfo);
            SerializeEngineParameters(writer, frame.EngineParameters, frame.PlayerInfo.Profile.GameMode);
            SerializeStats(writer, frame.Stats, frame.PlayerInfo.Profile.GameMode);

            writer.Write(frame.InputCount);
            foreach (var input in frame.Inputs)
            {
                writer.Write(input.Time);
                writer.Write(input.Action);
                writer.Write(input.Integer);
            }
        }

        private static ReplayFrame DeserializeFrame(BinaryReader reader, int version = 0)
        {
            var frame = new ReplayFrame();
            frame.PlayerInfo = DeserializePlayerInfo(reader, version);
            frame.EngineParameters = DeserializeEngineParameters(reader, frame.PlayerInfo.Profile.GameMode, version);
            frame.Stats = DeserializeStats(reader, frame.PlayerInfo.Profile.GameMode, version);

            frame.InputCount = reader.ReadInt32();
            frame.Inputs = new GameInput[frame.InputCount];
            for (int i = 0; i < frame.InputCount; i++)
            {
                var time = reader.ReadDouble();
                var action = reader.ReadInt32();
                var value = reader.ReadInt32();

                frame.Inputs[i] = new GameInput(time, action, value);
            }

            // Event logger was removed in version 6+
            if (version <= 5)
            {
                var eventLogger = new EngineEventLogger();
                eventLogger.Deserialize(reader, version);
            }
            return frame;
        }

        #endregion

        #region Player Info

        private static void SerializePlayerInfo(BinaryWriter writer, ReplayPlayerInfo playerInfo)
        {
            writer.Write(playerInfo.PlayerId);
            writer.Write(playerInfo.ColorProfileId);
            playerInfo.Profile.Serialize(writer);
        }

        private static ReplayPlayerInfo DeserializePlayerInfo(BinaryReader reader, int version = 0)
        {
            var playerInfo = new ReplayPlayerInfo();
            playerInfo.Deserialize(reader, version);
            return playerInfo;
        }

        #endregion

        #region Engine Parameters

        private static void SerializeEngineParameters(BinaryWriter writer, BaseEngineParameters engineParameters, GameMode gameMode)
        {
            engineParameters.HitWindow.Serialize(writer);
            writer.Write(engineParameters.MaxMultiplier);
            writer.Write(engineParameters.StarMultiplierThresholds.Length);
            foreach (var f in engineParameters.StarMultiplierThresholds)
            {
                writer.Write(f);
            }
            writer.Write(engineParameters.SongSpeed);

            switch (gameMode)
            {
                case GameMode.FiveFretGuitar:
                case GameMode.SixFretGuitar:
                    ReplayInstrumentSerializer.SerializeGuitarParameters(writer, (engineParameters as GuitarEngineParameters)!);
                    break;
                case GameMode.FourLaneDrums:
                case GameMode.FiveLaneDrums:
                    ReplayInstrumentSerializer.SerializeDrumsParameters(writer, (engineParameters as DrumsEngineParameters)!);
                    break;
                case GameMode.ProGuitar:
                    break;
                case GameMode.ProKeys:
                    ReplayInstrumentSerializer.SerializeProKeysParameters(writer, (engineParameters as ProKeysEngineParameters)!);
                    break;
                case GameMode.Vocals:
                    ReplayInstrumentSerializer.SerializeVocalsParameters(writer, (engineParameters as VocalsEngineParameters)!);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static BaseEngineParameters DeserializeEngineParameters(BinaryReader reader, GameMode gameMode,
            int version = 0)
        {
            var hitWindow = new HitWindowSettings();
            hitWindow.Deserialize(reader, version);

            int maxMultiplier = reader.ReadInt32();
            float[] starMultiplierThresholds = new float[reader.ReadInt32()];
            for (int i = 0; i < starMultiplierThresholds.Length; i++)
            {
                starMultiplierThresholds[i] = reader.ReadSingle();
            }

            double songSpeed = 1;
            if (version >= 5)
            {
                songSpeed = reader.ReadDouble();
            }

            BaseEngineParameters engineParameters = null!;
            switch (gameMode)
            {
                case GameMode.FiveFretGuitar:
                case GameMode.SixFretGuitar:
                    engineParameters = ReplayInstrumentSerializer.DeserializeGuitarParameters(reader, version);
                    break;
                case GameMode.FourLaneDrums:
                case GameMode.FiveLaneDrums:
                    engineParameters = ReplayInstrumentSerializer.DeserializeDrumsParameters(reader, version);
                    break;
                case GameMode.ProGuitar:
                    break;
                case GameMode.ProKeys:
                    engineParameters = ReplayInstrumentSerializer.DeserializeProKeysParameters(reader, version);
                    break;
                case GameMode.Vocals:
                    engineParameters = ReplayInstrumentSerializer.DeserializeVocalsParameters(reader, version);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null);
            }

            engineParameters.HitWindow = hitWindow;
            engineParameters.MaxMultiplier = maxMultiplier;
            engineParameters.StarMultiplierThresholds = starMultiplierThresholds;
            engineParameters.SongSpeed = songSpeed;

            return engineParameters;
        }

        #endregion

        #region Stats

        public static void SerializeStats(BinaryWriter writer, BaseStats stats, GameMode gameMode)
        {
            writer.Write(stats.CommittedScore);
            writer.Write(stats.PendingScore);
            writer.Write(stats.Combo);
            writer.Write(stats.MaxCombo);
            writer.Write(stats.ScoreMultiplier);
            writer.Write(stats.NotesHit);
            writer.Write(stats.TotalNotes);
            writer.Write(stats.StarPowerAmount);
            writer.Write(stats.StarPowerBaseAmount);
            writer.Write(stats.IsStarPowerActive);
            writer.Write(stats.StarPowerPhrasesHit);
            writer.Write(stats.TotalStarPowerPhrases);
            writer.Write(stats.SoloBonuses);
            writer.Write(stats.Stars);
        }

        public static BaseStats DeserializeStats(BinaryReader reader, GameMode gameMode, int version = 0)
        {
            var committedScore = reader.ReadInt32();
            var pendingScore = reader.ReadInt32();
            var combo = reader.ReadInt32();
            var maxCombo = reader.ReadInt32();
            var scoreMultiplier = reader.ReadInt32();
            var notesHit = reader.ReadInt32();
            var totalNotes = reader.ReadInt32();
            var starPowerAmount = reader.ReadDouble();
            var starPowerBaseAmount = reader.ReadDouble();
            var isStarPowerActive = reader.ReadBoolean();
            var starPowerPhrasesHit = reader.ReadInt32();
            var totalStarPowerPhrases = reader.ReadInt32();
            var soloBonuses = reader.ReadInt32();

            BaseStats stats = null!;
            switch (gameMode)
            {
                case GameMode.FiveFretGuitar:
                case GameMode.SixFretGuitar:
                    stats = new GuitarStats();
                    ReplayInstrumentSerializer.DeserializeGuitarStats(reader, version);
                    break;
                case GameMode.FourLaneDrums:
                case GameMode.FiveLaneDrums:
                    stats = new DrumsStats();
                    ReplayInstrumentSerializer.DeserializeDrumsStats(reader, version);
                    break;
                case GameMode.ProGuitar:
                    break;
                case GameMode.ProKeys:
                    stats = new ProKeysStats();
                    ReplayInstrumentSerializer.DeserializeProKeysStats(reader, version);
                    break;
                case GameMode.Vocals:
                    stats = new VocalsStats();
                    ReplayInstrumentSerializer.DeserializeVocalsStats(reader, version);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null);
            }

            stats.CommittedScore = committedScore;
            stats.PendingScore = pendingScore;
            stats.Combo = combo;
            stats.MaxCombo = maxCombo;
            stats.ScoreMultiplier = scoreMultiplier;
            stats.NotesHit = notesHit;
            stats.TotalNotes = totalNotes;
            stats.StarPowerAmount = starPowerAmount;
            stats.StarPowerBaseAmount = starPowerBaseAmount;
            stats.IsStarPowerActive = isStarPowerActive;
            stats.StarPowerPhrasesHit = starPowerPhrasesHit;
            stats.TotalStarPowerPhrases = totalStarPowerPhrases;
            stats.SoloBonuses = soloBonuses;

            return stats;
        }

        #endregion

    }
}