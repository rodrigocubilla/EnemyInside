using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnalyticsEvent = Unity.Services.Analytics.Event;

public class EventManager : MonoBehaviour
{
    public class LevelStartEvent : AnalyticsEvent
    {
        public LevelStartEvent() : base("LevelStart") { }
        public int level { set { SetParameter("level", value); } }
    }
    public class LevelCompleteEvent : AnalyticsEvent
    {
        public LevelCompleteEvent() : base("LevelComplete") { }
        public int level { set { SetParameter("level", value); } }
        public int live { set { SetParameter("live", value); } }
    }
    public class ResetEvent : AnalyticsEvent
    {
        public ResetEvent() : base("Reset") { }
        public int level { set { SetParameter("level", value); } }
        public bool move { set { SetParameter("move", value); } }
    }
    public class DeathEvent : AnalyticsEvent
    {
        public DeathEvent() : base("Death") { }
        public string mode { set { SetParameter("mode", value); } }
        public string killZone { set { SetParameter("killZone", value); } }
    }
    public class HumanEvent : AnalyticsEvent
    {
        public HumanEvent() : base("Human") { }
        public int level { set { SetParameter("level", value); } }
        public int count { set { SetParameter("count", value); } }
    }
    public class QuitEvent : AnalyticsEvent
    {
        public QuitEvent() : base("Quit") { }
        public int level { set { SetParameter("level", value); } }
    }
    public class GameOver : AnalyticsEvent
    {
        public GameOver() : base("GameOver") { }
        public int live { set { SetParameter("live", value); } }
    }
}
