﻿using System;
using YARG.Core.Chart;
using YARG.Core.Input;

namespace YARG.Core.Engine.Vocals.Engines
{
    public class YargVocalsEngine : VocalsEngine
    {
        public YargVocalsEngine(InstrumentDifficulty<VocalNote> chart, SyncTrack syncTrack, VocalsEngineParameters engineParameters)
            : base(chart, syncTrack, engineParameters)
        {
        }

        protected override void MutateStateWithInput(GameInput gameInput)
        {
            var action = gameInput.GetAction<VocalsAction>();

            if (action is VocalsAction.Hit && gameInput.Button)
            {
                State.HasHit = true;
            }
            else if (action is VocalsAction.Pitch)
            {
                State.HasSang = true;
                State.PitchSang = gameInput.Axis;

                OnSing?.Invoke(true);
            }
        }

        protected override void UpdateHitLogic(double time)
        {
            UpdateTimeVariables(time);
            UpdateStarPower();

            // Quit early if there are no notes left
            if (State.NoteIndex >= Notes.Count)
            {
                State.HasSang = false;
                return;
            }

            var phrase = Notes[State.NoteIndex];
            State.PhraseTicksTotal ??= GetTicksInPhrase(phrase);

            CheckForNoteHit();

            // Check for the end of a phrase
            if (State.CurrentTick > phrase.TickEnd)
            {
                var percentHit = State.PhraseTicksHit / State.PhraseTicksTotal.Value;
                if (State.PhraseTicksTotal.Value == 0)
                {
                    percentHit = 1.0;
                }

                bool hit = percentHit >= EngineParameters.PhraseHitPercent;
                if (hit)
                {
                    EngineStats.TicksHit += State.PhraseTicksTotal.Value;
                    HitNote(phrase);
                }
                else
                {
                    var ticksHit = (uint) Math.Round(State.PhraseTicksHit);

                    EngineStats.TicksHit += ticksHit;
                    EngineStats.TicksMissed += State.PhraseTicksTotal.Value - ticksHit;

                    MissNote(phrase, percentHit);
                }

                State.PhraseTicksHit = 0;
                State.PhraseTicksTotal = null;

                OnPhraseHit?.Invoke(percentHit / EngineParameters.PhraseHitPercent, hit);
            }
        }

        protected override void CheckForNoteHit()
        {
            CheckSingingHit();
            CheckHittingHit();
        }

        private void CheckSingingHit()
        {
            if (!State.HasSang)
            {
                return;
            }

            State.HasSang = false;
            var lastSingTick = State.LastSingTick;
            State.LastSingTick = State.CurrentTick;

            // If the last sing detected was on the same tick (or less), skip it
            // since we've already handled that tick.
            if (lastSingTick >= State.CurrentTick)
            {
                return;
            }

            var phrase = Notes[State.NoteIndex];

            // Get the note that we should currently be targeting
            var note = GetNoteInPhraseAtSongTick(phrase, State.CurrentTick);
            if (note is null)
            {
                // If we're not on a note, we cannot be hitting a note
                OnHit?.Invoke(false);

                return;
            }

            OnTargetNoteChanged?.Invoke(note);

            // This will never be a percussion note here
            if (CanVocalNoteBeHit(note, out float hitPercent))
            {
                // We will apply a leniency here and assume that it will also hit all other
                // ticks, since the user cannot change pitch between inputs.
                var maxLeniency = 1.0 / EngineParameters.ApproximateVocalFps;
                var lastTick = Math.Max(
                    SyncTrack.TimeToTick(State.CurrentTime - maxLeniency),
                    lastSingTick);

                var ticksSinceLast = State.CurrentTick - lastTick;
                State.PhraseTicksHit += ticksSinceLast * hitPercent;

                OnHit?.Invoke(true);
            }
            else
            {
                OnHit?.Invoke(false);
            }
        }

        private void CheckHittingHit()
        {
            if (!State.HasHit)
            {
                return;
            }

            State.HasHit = false;

            var phrase = Notes[State.NoteIndex];
            var note = GetNextPercussionNote(phrase, State.CurrentTick);

            if (note is null)
            {
                if (EngineStats.CanStarPowerActivate)
                {
                    ActivateStarPower();
                }

                return;
            }

            if (IsNoteInWindow(note))
            {
                HitNote(note);
            }
            else
            {
                // TODO: Some kind of overhitting
            }
        }

        protected override void UpdateBot(double songTime)
        {
            throw new NotImplementedException();
        }

        protected override bool CanVocalNoteBeHit(VocalNote note, out float hitPercent)
        {
            // If it is non-pitched, it is always hittable
            if (note.IsNonPitched)
            {
                hitPercent = 1f;
                return true;
            }

            var expectedPitch = note.PitchAtSongTime(State.CurrentTime);

            // Formula for calculating the distance to the expected pitch, while ignoring octaves
            float distanceToExpected = Math.Min(
                Mod(State.PitchSang - expectedPitch, 12f),
                Mod(expectedPitch - State.PitchSang, 12f));

            // If it is within the full points window, award full points
            if (distanceToExpected <= EngineParameters.PitchWindowPerfect)
            {
                hitPercent = 1f;
                return true;
            }

            // If it is outside of the total pitch window, then award no points
            if (distanceToExpected > EngineParameters.PitchWindow)
            {
                hitPercent = 0f;
                return false;
            }

            hitPercent = YargMath.InverseLerpF(
                EngineParameters.PitchWindow,
                EngineParameters.PitchWindowPerfect,
                distanceToExpected);
            return true;

            // Positive remainder
            static float Mod(float a, float b)
            {
                var remainder = a % b;
                if (remainder < 0)
                {
                    if (b < 0)
                    {
                        return remainder - b;
                    }

                    return remainder + b;
                }

                return remainder;
            }
        }

        protected override bool CanNoteBeHit(VocalNote note) => throw new NotImplementedException();
    }
}