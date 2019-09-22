#### Component Width and Height

The `Width` and `Height` controls the component dimensions on the layout.

They can be **absolute values**, where component will take the specified size regardless of it's content:
```!SNIPPET
<Form>
  <Panel Width="200" Height="200" BackgroundColor="red">
    <Panel Width="100" Height="150" BackgroundColor="green">
      <Label Width="60" Height="100" Text="Hi Bob! How are you my friend?" Font="Calibri;15" TextColor="white"/>
    </Panel>
  </Panel>
</Form>
```

They can be not specified or just `-`, so component will take the minimum required size to render it's content:
```!SNIPPET
<Form>
  <Panel Width="-" Height="-" BackgroundColor="red">
    <Panel BackgroundColor="green">
      <Label Text="Hi Bob! How are you my friend?" Font="Calibri;15" TextColor="white"/>
    </Panel>
  </Panel>
</Form>
```

*In this case the red panel is not visible as it's entire content is taken by child panel.*

They can be mixed as well:
```!SNIPPET
<Form>
  <Panel Width="-" Height="200" BackgroundColor="red">
    <Panel Width="80" BackgroundColor="green">
      <Label Text="Hi Bob! How are you my friend?" Font="Calibri;15" TextColor="white"/>
    </Panel>
  </Panel>
</Form>
```

Finally, they can be `*`, so component would take remaining available space:
```!SNIPPET
<Form>
  <Panel Width="400" Height="100" BackgroundColor="red">
    <Panel BackgroundColor="green" Width="*">
      <Label Text="Hi Bob! How are you my friend?" Font="Calibri;15" TextColor="white"/>
    </Panel>
  </Panel>
</Form>
```

#### Component Alignment

The `Alignment` property controls how the component should be placed within the container if it's size is smaller than available place.
By default it will be aligned top-left, but it can be changed:

```!SNIPPET
<Form>
  <Panel Width="300" Height="200">
    <Panel Width="200" Height="150" Alignment="bottom right" BackgroundColor="red">
      <Panel Width="100" Height="130" Alignment="center left" BackgroundColor="green">
        <Label Width="60" Height="80" Alignment="bottom center" Text="Hi Bob! How are you my friend?" Font="Calibri;15" TextColor="white"/>
      </Panel>
    </Panel>
  </Panel>
</Form>
```

#### When component is too big

It may happen that component dimensions are bigger than the container space. In such situation the content will be clipped as the library will try to not allow content overlaying outside of allowed dimensions.

```!SNIPPET
<Form>
  <Panel Width="80" Height="120" BackgroundColor="red">
    <Panel Width="100" Height="100" Alignment="center left" BackgroundColor="green">
      <Label Width="100" Height="60" Text="Hi Bob! How are you my friend?" Font="Calibri;15" TextColor="white"/>
    </Panel>
  </Panel>
</Form>
```

#### Other cases

While the `Component` itself is generally useless except the scenario where it is needed to introduce spaces between other components - please [HBox](#hbox) and [VBox](#vbox) containers for details.