using System;

namespace Tic_Tac_Toe
{
    internal static class PlayerMarkExtension
    {
        public static char ToSymbol(this PlayerMark playerMark)
        {
            switch (playerMark)
            {
                case PlayerMark.X_Player:
                    return 'X';
                case PlayerMark.O_Player:
                    return 'O';
                default:
                    return '-';
            }
        }
    }
}
