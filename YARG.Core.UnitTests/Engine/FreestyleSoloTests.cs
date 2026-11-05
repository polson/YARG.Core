using System.Collections.Generic;
using NUnit.Framework;
using YARG.Core.Chart;
using YARG.Core.Engine;
using YARG.Core.Engine.Guitar;
using YARG.Core.Engine.Guitar.Engines;
using YARG.Core.Input;

namespace YARG.Core.UnitTests.Engine;

public class FreestyleSoloTests
{
    private const double TICKS_PER_SEC = 960.0;

    private static GuitarNote Note(FiveFretGuitarFret fret, uint tick, uint tickLength, NoteFlags flags)
    {
        return new GuitarNote(fret, GuitarNoteType.Strum, GuitarNoteFlags.None, flags,
            tick / TICKS_PER_SEC, tickLength / TICKS_PER_SEC, tick, tickLength);
    }

    private static YargFiveFretGuitarEngine CreateEngine(bool freestyleSolos, List<int> misses)
    {
        var notes = new[]
        {
            Note(FiveFretGuitarFret.Green, 0, 0, NoteFlags.None),
            Note(FiveFretGuitarFret.Red, 480, 0, NoteFlags.Solo | NoteFlags.SoloStart),
            Note(FiveFretGuitarFret.Yellow, 720, 0, NoteFlags.Solo),
            Note(FiveFretGuitarFret.Blue, 960, 240, NoteFlags.Solo | NoteFlags.SoloEnd),
            Note(FiveFretGuitarFret.Green, 1920, 0, NoteFlags.None),
        };

        for (int i = 0; i < notes.Length; i++)
        {
            if (i > 0) notes[i].PreviousNote = notes[i - 1];
            if (i < notes.Length - 1) notes[i].NextNote = notes[i + 1];
        }

        var difficulty = new InstrumentDifficulty<GuitarNote>(Instrument.FiveFretGuitar, Difficulty.Expert,
            new(notes), new(), new());

        var thresholds = new[] { 0.08f, 0.17f, 0.28f, 0.41f, 0.55f, 0.71f, 0.88f, 1.0f };
        var engineParams = new GuitarEngineParameters(
            new HitWindowSettings(0.1, 0.1, 1.0, false, 0, 1.0, 1.0, 0.15, 0.25),
            4, 0, 0.05, thresholds, thresholds,
            0.1, 0.1, 0.1,
            false, true, false, false, true, freestyleSolos);

        var syncTrack = new SyncTrack(480);
        syncTrack.Tempos.Add(new TempoChange(120, 0, 0));

        var engine = new YargFiveFretGuitarEngine(difficulty, syncTrack, engineParams, false);
        engine.OnNoteMissed += (index, _) => misses.Add(index);
        return engine;
    }

    private static void Play(YargFiveFretGuitarEngine engine)
    {
        var inputs = new List<GameInput>
        {
            GameInput.Create(0.0, GuitarAction.GreenFret, true),
            GameInput.Create(0.0, GuitarAction.StrumDown, true),
            GameInput.Create(0.02, GuitarAction.StrumDown, false),
            GameInput.Create(0.3, GuitarAction.OrangeFret, true),
            GameInput.Create(0.6, GuitarAction.StrumDown, true),
            GameInput.Create(0.62, GuitarAction.StrumDown, false),
            GameInput.Create(0.9, GuitarAction.StrumUp, true),
            GameInput.Create(0.92, GuitarAction.StrumUp, false),
            GameInput.Create(1.5, GuitarAction.OrangeFret, false),
            GameInput.Create(2.0, GuitarAction.StrumDown, true),
            GameInput.Create(2.02, GuitarAction.StrumDown, false),
        };

        int next = 0;
        for (double time = 0; time <= 2.5; time += 1.0 / 60.0)
        {
            while (next < inputs.Count && inputs[next].Time <= time)
            {
                var input = inputs[next++];
                engine.QueueInput(ref input);
            }

            engine.Update(time);
        }
    }

    [Test]
    public void FreestyleSolo_HitsHiddenNotes_AndIgnoresPlayerStrums()
    {
        var misses = new List<int>();
        var engine = CreateEngine(true, misses);

        Play(engine);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(misses, Is.Empty);
            Assert.That(engine.EngineStats.Overstrums, Is.Zero);
            Assert.That(engine.EngineStats.NotesHit, Is.EqualTo(5));
            Assert.That(engine.EngineStats.MaxCombo, Is.EqualTo(5));
            Assert.That(engine.EngineStats.SoloBonuses, Is.GreaterThan(0));
            Assert.That(engine.IsFreestyleActive, Is.False);
        }
    }

    [Test]
    public void WithoutFreestyle_SameInputsMissTheSolo()
    {
        var misses = new List<int>();
        var engine = CreateEngine(false, misses);

        Play(engine);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(misses, Is.Not.Empty);
            Assert.That(engine.EngineStats.NotesHit, Is.LessThan(5));
        }
    }
}
