//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.10
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


using System;
using System.Runtime.InteropServices;

namespace Noesis
{

public class DataObjectEventArgs : RoutedEventArgs {
  private HandleRef swigCPtr;

  internal DataObjectEventArgs(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn) {
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(DataObjectEventArgs obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~DataObjectEventArgs() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          NoesisGUI_PINVOKE.delete_DataObjectEventArgs(swigCPtr);
        }
        swigCPtr = new HandleRef(null, IntPtr.Zero);
      }
      GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public bool CommandCancelled {
    get {
      bool ret = NoesisGUI_PINVOKE.DataObjectEventArgs_CommandCancelled_get(swigCPtr);
      return ret;
    } 
  }

  public bool IsDragDrop {
    get {
      bool ret = NoesisGUI_PINVOKE.DataObjectEventArgs_IsDragDrop_get(swigCPtr);
      return ret;
    } 
  }

  public void CancelCommand() {
    NoesisGUI_PINVOKE.DataObjectEventArgs_CancelCommand(swigCPtr);
  }

}

}

