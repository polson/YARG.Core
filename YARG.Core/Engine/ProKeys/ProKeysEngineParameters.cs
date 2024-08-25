﻿using System.IO;

namespace YARG.Core.Engine.ProKeys
{
    public class ProKeysEngineParameters : BaseEngineParameters
    {
        public double ChordStaggerWindow { get; private set; }

        public double FatFingerWindow { get; private set; }

        public ProKeysEngineParameters()
        {
        }

        public ProKeysEngineParameters(HitWindowSettings hitWindow, int maxMultiplier, double spWhammyBuffer,
            double sustainDropLeniency, float[] starMultiplierThresholds, double chordStaggerWindow, double fatFingerWindow)
            : base(hitWindow, maxMultiplier, spWhammyBuffer, sustainDropLeniency, starMultiplierThresholds)
        {
            ChordStaggerWindow = chordStaggerWindow;
            FatFingerWindow = fatFingerWindow;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);

            writer.Write(ChordStaggerWindow);
            writer.Write(FatFingerWindow);
        }

        public override void Deserialize(BinaryReader reader, int version = 0)
        {
            base.Deserialize(reader, version);

            ChordStaggerWindow = reader.ReadDouble();
            FatFingerWindow = reader.ReadDouble();
        }

        public override string ToString()
        {
            return
                $"{base.ToString()}\n" +
                $"Chord stagger window: {ChordStaggerWindow}\n" +
                $"Fat finger window: {FatFingerWindow}";
        }
    }
}