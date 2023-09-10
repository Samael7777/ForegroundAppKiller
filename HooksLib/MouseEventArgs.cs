//------------------------------------------------------------------------------
// <copyright file="MouseEvent.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>                                                                
//------------------------------------------------------------------------------

using System;
using System.Drawing;


namespace HooksLib
{
    public class MouseEventArgs : EventArgs {
        /// <devdoc>
        ///    <para>
        ///       Initializes a new instance of the <see cref='MouseEventArgs'/> class.
        ///    </para>
        /// </devdoc>
        public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta) 
        {
            this.Button = button;
            this.Clicks = clicks;
            this.X = x;
            this.Y = y;
            this.Delta = delta;
        }
 
        /// <devdoc>
        ///    <para>
        ///       Gets which mouse button was pressed.
        ///    </para>
        /// </devdoc>
        public MouseButtons Button { get; }

        /// <devdoc>
        ///    <para>
        ///       Gets the
        ///       number of times the mouse
        ///       button was pressed and released.
        ///    </para>
        /// </devdoc>
        public int Clicks { get; }

        /// <devdoc>
        ///    <para>
        ///       Gets the x-coordinate
        ///       of a mouse click.
        ///    </para>
        /// </devdoc>
        public int X { get; }

        /// <devdoc>
        ///    <para>
        ///       Gets the y-coordinate of a mouse click.
        ///    </para>
        /// </devdoc>
        public int Y { get; }

        /// <devdoc>
        ///    <para>
        ///       Gets
        ///       a signed count of the number of detents the mouse wheel has rotated.
        ///    </para>
        /// </devdoc>
        public int Delta { get; }

        /// <devdoc>
        ///    <para>
        ///       Gets the location of the mouse during MouseEvent.
        ///    </para>
        /// </devdoc>
        public Point Location => new Point(X,Y);
    }
}