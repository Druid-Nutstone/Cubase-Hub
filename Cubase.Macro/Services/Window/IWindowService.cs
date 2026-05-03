using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Services.Window
{
    public interface IWindowService
    {
        bool IsCubaseActive(bool log = true);

        bool IsCubaseRunning();

        bool IsCubaseMainWindowActive();

        bool BringCubaseToFront();

        Rectangle GetCubaseBounds();

        void PositionCubase(int left);

        void MaximiseCubase();

        bool IsCubasePositioned(int left);

        ExternalWindowState GetCubaseWindowState();

        IntPtr GetCurrentForeGroundWindow();

        // static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);
    }
}
