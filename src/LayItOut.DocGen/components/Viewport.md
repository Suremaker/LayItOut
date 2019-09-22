The use case is to display only a part of the bigger component, allowing to clip it from any side.

```!SNIPPET
<Form>
  <VBox>
    <Viewport Width="300" Height="300" ContentAlignment="top left">
      <Image Src="image.png" Width="600" Scaling="Uniform" />
    </Viewport>
    <Viewport Width="300" Height="300" ContentAlignment="bottom right">
      <Image Src="image.png" Width="600" Scaling="Uniform" />
    </Viewport>
    <Viewport Width="300" Height="300" ContentAlignment="center">
      <Image Src="image.png" Width="600" Scaling="Uniform" />
    </Viewport>
  </VBox>
</Form>
```

It also allows to specify clip margins:

```!SNIPPET
<Form>
  <VBox>
    <Viewport Width="300" Height="300" ContentAlignment="bottom right" ClipMargin="0">
      <Image Src="image.png" Width="600" Scaling="Uniform" />
    </Viewport>
    <Viewport Width="300" Height="300" ContentAlignment="bottom right" ClipMargin="50">
      <Image Src="image.png" Width="600" Scaling="Uniform" />
    </Viewport>
    <Viewport Width="300" Height="300" ContentAlignment="bottom right" ClipMargin="0 0 150 50">
      <Image Src="image.png" Width="600" Scaling="Uniform" />
    </Viewport>
  </VBox>
</Form>
```
