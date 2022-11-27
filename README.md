# SketchOverlay
A simple screen drawing tool using dotnet Maui.

### Bump in the road - 28-11-2022
I began working on this app with two goals in mind: 
- To deliver an opensource alternative to subscription model onscreen drawing tools.
- To familiarise myself with the DotNet MAUI development process.

I've found the MAUI development process to be simple and rewarding as well as confusing and frustrating. My frustrations mainly stem from the fact that basic platform-specific functionality is not exposed through the primary API's and require a fair amount of research. With that said, I've obtained the know-how to implement the basic features. Except one...

WinUI 3 (the windows specific MAUI implementation) lacks support for transparent borderless windows.  
This should have been the first feature to implement, as it's arguably the most important part of an overlay.  
Unfortunately, I relied on my WPF experience and assumed that creating an overlay window would required a few lines of xaml and MAYBE a little win32 interop.

For now, I'll do the only thing that makes sense - migrate to WPF and hope for an update in the near future.

Feature request: https://github.com/microsoft/microsoft-ui-xaml/issues/1247
