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

public class HitTest3DResult : IDisposable {
  private HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal HitTest3DResult(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(HitTest3DResult obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~HitTest3DResult() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          NoesisGUI_PINVOKE.delete_HitTest3DResult(swigCPtr);
        }
        swigCPtr = new HandleRef(null, IntPtr.Zero);
      }
      GC.SuppressFinalize(this);
    }
  }

  public DependencyObject VisualHit {
    get { return GetVisualHit(); }
  }

  public Point3D WorldPos {
    get { return GetWorldPos(); }
  }

  public HitTest3DResult() : this(NoesisGUI_PINVOKE.new_HitTest3DResult(), true) {
  }

  private DependencyObject GetVisualHit() {
    IntPtr cPtr = NoesisGUI_PINVOKE.HitTest3DResult_GetVisualHit(swigCPtr);
    return (DependencyObject)Noesis.Extend.GetProxy(cPtr, false);
  }

  private Point3D GetWorldPos() {
    IntPtr ret = NoesisGUI_PINVOKE.HitTest3DResult_GetWorldPos(swigCPtr);
    if (ret != IntPtr.Zero) {
      return Marshal.PtrToStructure<Point3D>(ret);
    }
    else {
      return new Point3D();
    }
  }

}

}
