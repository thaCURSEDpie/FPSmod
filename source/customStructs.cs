using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using GTA;

namespace killNotifications
{
    public struct killEvent
    {
        public string eventType;
        public int eventTime;
        public bool eventInVehicle;
    }

    public struct killNotification
    {
        public string Attacker;
        public string Victim;
        public string Weapon;
        public bool isVisible;
        public float timeLimit;
    };
}