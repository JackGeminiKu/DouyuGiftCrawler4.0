using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DouyuGiftCrawler
{
    public static class DouyuService
    {
        static int _currentRoom = 0;
        static readonly object _locker = new object();

        public static int NextRoom()
        {
            lock (_locker) {
                return _currentRoom++;
            }
        }

        public static string GetRoomApiUrl(int roomNumber)
        {
            return @"http://open.douyucdn.cn/api/RoomApi/room/" + roomNumber;
        }

        public static void LoadSession()
        {
            _currentRoom = Properties.Settings.Default.CurrentRoom;
        }

        public static void SaveSession()
        {
            Properties.Settings.Default.CurrentRoom = _currentRoom;
            Properties.Settings.Default.Save();
        }
    }



    /// <summary>
    /// room number event args
    /// </summary>
    public class RoomNnumberEventArgs : EventArgs
    {
        public RoomNnumberEventArgs(int roomNumber)
        {
            RoomNumber = roomNumber;
        }

        public int RoomNumber { get; private set; }
    }
}
