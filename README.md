# CVBImageProc
Tool for image editing using the [Common Vision Blox](https://www.commonvisionblox.com/en/) image processing SDK.

## Running
To run this software you will need to [download and install Common Vision Blox](https://forum.commonvisionblox.com/c/downloads/5).

## Processors
Image editing is done by building a processor chain, each processor applying a different edit to the image.

Currently available processors:
- Binarise
- Bit Shift
- Crop
- Filter
- Invert
- Math
- Mono To Multiplane
- Pixel Shift
- Plane Clear
- Resize
- Replace
- RGB Factors
- RGB To Mono
- Rotate
- Shuffle
- Smear
- Sort

### Pixel Filter
Most processors support configuring which pixels are edited by them. For example "only every 2nd pixel" or "only pixels with value > 200".
Pixel filters are added to a pixel filter chain which can be used in "And" or "Or" logic mode for more configuration options.
Each pixel filters logic can be inverted.

Currently available pixel filters:
- Equals (Index)
- Equals (Value)
- Larger Than (Index)
- Larger Than (Value)
- Modulo (Index)
- Modulo (Value)
- Randomize
