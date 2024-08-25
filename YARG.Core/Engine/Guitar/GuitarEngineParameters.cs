﻿using System.IO;

namespace YARG.Core.Engine.Guitar
{
    public class GuitarEngineParameters : BaseEngineParameters
    {
        public double HopoLeniency { get; private set; }

        public double StrumLeniency      { get; private set; }
        public double StrumLeniencySmall { get; private set; }

        public bool InfiniteFrontEnd { get; private set; }
        public bool AntiGhosting     { get; private set; }

        public GuitarEngineParameters()
        {
        }

        public GuitarEngineParameters(HitWindowSettings hitWindow, int maxMultiplier, double spWhammyBuffer,
            double sustainDropLeniency, float[] starMultiplierThresholds, double hopoLeniency, double strumLeniency,
            double strumLeniencySmall, bool infiniteFrontEnd, bool antiGhosting)
            : base(hitWindow, maxMultiplier, spWhammyBuffer, sustainDropLeniency, starMultiplierThresholds)
        {
            HopoLeniency = hopoLeniency;

            StrumLeniency = strumLeniency;
            StrumLeniencySmall = strumLeniencySmall;

            InfiniteFrontEnd = infiniteFrontEnd;
            AntiGhosting = antiGhosting;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);

            writer.Write(HopoLeniency);

            writer.Write(StrumLeniency);
            writer.Write(StrumLeniencySmall);

            writer.Write(InfiniteFrontEnd);
            writer.Write(AntiGhosting);
        }

        public override void Deserialize(BinaryReader reader, int version = 0)
        {
            base.Deserialize(reader, version);

            HopoLeniency = reader.ReadDouble();

            StrumLeniency = reader.ReadDouble();
            StrumLeniencySmall = reader.ReadDouble();

            InfiniteFrontEnd = reader.ReadBoolean();
            AntiGhosting = reader.ReadBoolean();
        }

        public override string ToString()
        {
            return
                $"{base.ToString()}\n" +
                $"Infinite front-end: {InfiniteFrontEnd}\n" +
                $"Anti-ghosting: {AntiGhosting}\n" +
                $"Hopo leniency: {HopoLeniency}\n" +
                $"Strum leniency: {StrumLeniency}\n" +
                $"Strum leniency (small): {StrumLeniencySmall}\n" +
                $"Star power whammy buffer: {StarPowerWhammyBuffer}";
        }
    }
}