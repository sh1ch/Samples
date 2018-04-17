using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaCalculator.Models
{
    /// <summary>
    /// <see cref="PlotGestureControllerExtension"/> クラスは、プロットのマウス操作を設定するクラスです。
    /// </summary>
    public static class PlotGestureControllerExtension
    {
        #region Public Methods

        /// <summary>
        /// グラフのマウス操作、キー操作を初期化します。
        /// </summary>
        /// <param name="gestureController">マウス操作のバインド。</param>
        public static void InitializeBind(this PlotController gestureController)
        {
            // グラフのマウス操作およびキー操作の初期化
            gestureController.UnbindKeyDown(OxyKey.A);
            gestureController.UnbindKeyDown(OxyKey.C, OxyModifierKeys.Control);
            gestureController.UnbindKeyDown(OxyKey.C, OxyModifierKeys.Control | OxyModifierKeys.Alt);
            gestureController.UnbindKeyDown(OxyKey.R, OxyModifierKeys.Control | OxyModifierKeys.Alt);
            gestureController.UnbindTouchDown();
            gestureController.UnbindMouseDown(OxyMouseButton.Left);
            gestureController.UnbindMouseDown(OxyMouseButton.Middle);
            gestureController.UnbindMouseDown(OxyMouseButton.Right);

            gestureController.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            gestureController.BindMouseDown(OxyMouseButton.Middle, PlotCommands.PointsOnlyTrack);
            gestureController.BindMouseDown(OxyMouseButton.Right, PlotCommands.ZoomRectangle);
        }

        #endregion
    }
}
