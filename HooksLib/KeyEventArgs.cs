//------------------------------------------------------------------------------
// <copyright file="KeyEvent.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>                                                                
//------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace HooksLib
{
    public class KeyEventArgs : EventArgs {
        /// <include file='doc\KeyEvent.uex' path='docs/doc[@for="KeyEventArgs.suppressKeyPress"]/*' />
        /// <devdoc>
        /// </devdoc>
        private bool _suppressKeyPress = false;
 
        /// <include file='doc\KeyEvent.uex' path='docs/doc[@for="KeyEventArgs.KeyEventArgs"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Initializes a new
        ///       instance of the <see cref='System.Windows.Forms.KeyEventArgs'/> class.
        ///    </para>
        /// </devdoc>
        public KeyEventArgs(Keys keyData) {
            this.KeyData = keyData;
        }
 
        /// <include file='doc\KeyEvent.uex' path='docs/doc[@for="KeyEventArgs.Alt"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets a value indicating whether the ALT key was pressed.
        ///    </para>
        /// </devdoc>
        public virtual bool Alt => (KeyData & Keys.Alt) == Keys.Alt;

        /// <include file='doc\KeyEvent.uex' path='docs/doc[@for="KeyEventArgs.Control"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets a value indicating whether the CTRL key was pressed.
        ///    </para>
        /// </devdoc>
        public bool Control => (KeyData & Keys.Control) == Keys.Control;

        /// <include file='doc\KeyEvent.uex' path='docs/doc[@for="KeyEventArgs.Handled"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets or sets a value
        ///       indicating whether the event was handled.
        ///    </para>
        /// </devdoc>
        //
        public bool Handled { get; set; } = false;

        /// <include file='doc\KeyEvent.uex' path='docs/doc[@for="KeyEventArgs.KeyCode"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets the keyboard code for a <see cref='System.Windows.Forms.Control.KeyDown'/> or <see cref='System.Windows.Forms.Control.KeyUp'/>
        ///       event.
        ///    </para>
        /// </devdoc>
        //subhag : changed the behaviour of the KeyCode as per the new requirements.
        public Keys KeyCode {
            [
                // Keys is discontiguous so we have to use Enum.IsDefined.
                SuppressMessage("Microsoft.Performance", "CA1803:AvoidCostlyCallsWherePossible")
            ]
            get {
                Keys keyGenerated =  KeyData & Keys.KeyCode;
 
                // since Keys can be discontiguous, keeping Enum.IsDefined.
                if (!Enum.IsDefined(typeof(Keys),(int)keyGenerated))
                    return Keys.None;
                else
                    return keyGenerated;
            }
        }
 
        /// <include file='doc\KeyEvent.uex' path='docs/doc[@for="KeyEventArgs.KeyValue"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets the keyboard value for a <see cref='System.Windows.Forms.Control.KeyDown'/> or <see cref='System.Windows.Forms.Control.KeyUp'/>
        ///       event.
        ///    </para>
        /// </devdoc>
        //subhag : added the KeyValue as per the new requirements.
        public int KeyValue => (int)(KeyData & Keys.KeyCode);

        /// <include file='doc\KeyEvent.uex' path='docs/doc[@for="KeyEventArgs.KeyData"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets the key data for a <see cref='System.Windows.Forms.Control.KeyDown'/> or <see cref='System.Windows.Forms.Control.KeyUp'/>
        ///       event.
        ///    </para>
        /// </devdoc>
        public Keys KeyData { get; }

        /// <include file='doc\KeyEvent.uex' path='docs/doc[@for="KeyEventArgs.Modifiers"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets the modifier flags for a <see cref='System.Windows.Forms.Control.KeyDown'/> or <see cref='System.Windows.Forms.Control.KeyUp'/> event.
        ///       This indicates which modifier keys (CTRL, SHIFT, and/or ALT) were pressed.
        ///    </para>
        /// </devdoc>
        public Keys Modifiers => KeyData & Keys.Modifiers;

        /// <devdoc>
        ///    <para>
        ///       Gets
        ///       a value indicating whether the SHIFT key was pressed.
        ///    </para>
        /// </devdoc>
        public virtual bool Shift => (KeyData & Keys.Shift) == Keys.Shift;

        /// <devdoc>
        /// </devdoc>
        //
        public bool SuppressKeyPress {
            get => _suppressKeyPress;
            set {
                _suppressKeyPress = value;
                Handled = value;
            }
        }
 
    }
}