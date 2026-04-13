using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Services.Window
{
    public interface IWindowService
    {
        bool IsCubaseActive(bool log = true);

        bool BringCubaseToFront();

        Rectangle GetCubaseBounds();

        void PositionCubase(int left);

        void MaximiseCubase();
    }
}
