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
        public int gameOver { set { SetParameter("gameOver", value); } }
    }
    public class DeathEvent : AnalyticsEvent
    {
        public DeathEvent() : base("Death") { }
        public string mode { set { SetParameter("mode", value); } }
        public string killZone { set { SetParameter("killZone", value); } }
        public int level { set { SetParameter("level", value); } }
    }
    public class HumanEvent : AnalyticsEvent
    {
        public HumanEvent() : base("Human") { }
        public int level { set { SetParameter("level", value); } }
        public int count { set { SetParameter("count", value); } }
    }
    public class GameOverEvent : AnalyticsEvent
    {
        public GameOverEvent() : base("GameOver") { }
        public int level { set { SetParameter("level", value); } }
        public bool reset { set { SetParameter("reset", value); } }
    }
    public class GameFinishEvent : AnalyticsEvent
    {
        public GameFinishEvent() : base("GameFinish") { }
        public int level { set { SetParameter("level", value); } }
        public int gameOver { set { SetParameter("gameOver", value); } }
    }
}
