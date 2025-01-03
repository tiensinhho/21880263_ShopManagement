﻿#pragma checksum "..\..\..\..\GUI\LoginWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8B4F37B07ED1F9D82E73B2420CC59F7BDD945BA6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using _21880263;


namespace _21880263 {
    
    
    /// <summary>
    /// LoginWindow
    /// </summary>
    public partial class LoginWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\..\GUI\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox usernameTextBox;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\GUI\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox passwordTextBox;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\GUI\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chbRememberPassword;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\GUI\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button loginButton;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\GUI\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.PopupBox serverPopup;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\..\GUI\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox serverTextBox;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\GUI\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox databaseTextBox;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\GUI\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btSave;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\GUI\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btCancel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/EShop;component/gui/loginwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\GUI\LoginWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 15 "..\..\..\..\GUI\LoginWindow.xaml"
            ((_21880263.LoginWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 24 "..\..\..\..\GUI\LoginWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Window_Close);
            
            #line default
            #line hidden
            return;
            case 3:
            this.usernameTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.passwordTextBox = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 5:
            this.chbRememberPassword = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.loginButton = ((System.Windows.Controls.Button)(target));
            
            #line 51 "..\..\..\..\GUI\LoginWindow.xaml"
            this.loginButton.Click += new System.Windows.RoutedEventHandler(this.loginButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.serverPopup = ((MaterialDesignThemes.Wpf.PopupBox)(target));
            return;
            case 8:
            this.serverTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.databaseTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.btSave = ((System.Windows.Controls.Button)(target));
            
            #line 71 "..\..\..\..\GUI\LoginWindow.xaml"
            this.btSave.Click += new System.Windows.RoutedEventHandler(this.btSave_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.btCancel = ((System.Windows.Controls.Button)(target));
            
            #line 72 "..\..\..\..\GUI\LoginWindow.xaml"
            this.btCancel.Click += new System.Windows.RoutedEventHandler(this.btCancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

