The simple allows to embed the image using it's original size:

```!SNIPPET
<Form>
  <Image Src="image.png" />
</Form>
```

When `Width` or `Height` is specified, the image will be scaled (enlarged or shrunk) to fit to the specified dimensions:

```!SNIPPET
<Form>
  <Image Src="image.png" Width="40" Height="40" />
</Form>
```

The `Scaling` attribute controls the scaling behaviour, allowing to decide if the image has to be:
* scaled to fit the content (`Fill` value, which is default),
* scaled with preserving aspect ratio (`Uniform`),
* left as is (`None` option), which would lead to image being clipped from the bottom-right corner if get's too big.

```!SNIPPET
<Form>
  <VBox>
    <Image Src="image.png" Scaling="Fill" Width="300" Height="300" />
    <Image Src="image.png" Scaling="Uniform" Width="300" Height="300" />
    <Image Src="image.png" Scaling="None" Width="300" Height="300" />
  </VBox>
</Form>
```

In order to clip image from other sides, please use [Viewport](#viewport).

