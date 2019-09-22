The panel can be used in many different scenarios. 

It allows to specify a **background color** for an inner component:
```!SNIPPET
<Form>
  <Panel BackgroundColor="green">
    <Label Text="Hello world!" Font="Calibri;15" TextColor="white"/>
  </Panel>
</Form>
```

It allows to add **padding** around inner component as well, where padded space will contain the specified background color:
```!SNIPPET
<Form>
  <Panel BackgroundColor="green" Padding="5">
    <Label Text="Hello world!" Font="Calibri;15" TextColor="white"/>
  </Panel>
</Form>
```

As an alternative it is possible to use **margin**, if whole component should be rendered with a space (without the background color):
```!SNIPPET
<Form>
  <Panel BackgroundColor="green" Margin="5">
    <Label Text="Hello world!" Font="Calibri;15" TextColor="white"/>
  </Panel>
</Form>
```

The panel allows as well to draw a **border** around the component, which is located between the padding and margin areas:
```!SNIPPET
<Form>
  <Panel BackgroundColor="green" Margin="5" Padding="5" Border="2 red">
    <Label Text="Hello world!" Font="Calibri;15" TextColor="white"/>
  </Panel>
</Form>
```

The border can be **rounded** as well - it is a matter of specifying border radius:
```!SNIPPET
<Form>
  <Panel BackgroundColor="green" Margin="5" Padding="5" Border="2 red" BorderRadius="10">
    <Label Text="Hello world!" Font="Calibri;15" TextColor="white"/>
  </Panel>
</Form>
```

It is possible to use different values for different sides of the panel:
* Margin, Padding accepts also `[vertical] [horizontal]` or `[top] [left] [bottom] [right]`,
* BorderRadius accepts also `[top] [bottom]` or `[top left] [top right] [bottom right] [bottom left]`

```!SNIPPET
<Form>
  <Panel BackgroundColor="green" Margin="10 5 15 20" Padding="3 10 3 10" Border="2 red" BorderRadius="0 10 10 5">
    <Label Text="Hello world!" Font="Calibri;15" TextColor="white"/>
  </Panel>
</Form>
```

It is possible to use `*` for Margin and Padding, allowing to take remaining space (it would also work for the child Width/Height dimensions):

```!SNIPPET
<Form>
  <Panel Width="200" Height="100" BackgroundColor="green" Margin="*" Padding="10 5 10 *" Border="2 red" BorderRadius="5">
    <Label Text="Hello world!" Font="Calibri;15" TextColor="white"/>
  </Panel>
</Form>
```

Please note here, that horizontal space has been distributed evenly between three parts, left margin, right padding and right margin.  
Also it's worth to note that panel Width/Height has to be bigger than necessary (either absolute or `*`) as `*` works on the excessive spaces.

Finally, if panel dimensions are smaller than the sum of margin, border, padding and inner component size, then panel parts will get clipped starting from inner-element then going outwards up to the margin:
```!SNIPPET
<Form>
  <Panel Width="100" Height="100" BackgroundColor="green" Margin="20" Padding="20" Border="1 red">
    <Label Text="Hello world!" Font="Calibri;15" TextColor="white"/>
  </Panel>
</Form>
```