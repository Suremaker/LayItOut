### Basic VBox usage

VBox lays out the inner components vertically from top to bottom:

```!SNIPPET
<Form>
  <VBox>
    <Panel Width="50" Height="50" BackgroundColor="red" />
    <Panel Width="30" Height="30" BackgroundColor="green" />
    <Panel Width="40" Height="40" BackgroundColor="blue" />
  </VBox>
</Form>
```

By default, the VBox will try to expand to accommodate it's all components. If it's smaller than required size, it will clip the components from the bottom-right:
```!SNIPPET
<Form>
  <VBox Width="35" Height="85">
    <Panel Width="50" Height="50" BackgroundColor="red" />
    <Panel Width="30" Height="30" BackgroundColor="green" />
    <Panel Width="40" Height="40" BackgroundColor="blue" />
  </VBox>
</Form>
```

If it is bigger than required size, it will lay the components as is:
```!SNIPPET
<Form>
  <VBox Width="80" Height="200">
    <Panel Width="50" Height="50" BackgroundColor="red" />
    <Panel Width="30" Height="30" BackgroundColor="green" />
    <Panel Width="40" Height="40" BackgroundColor="blue" />
  </VBox>
</Form>
```

It honors components with `unlimited size`, distributing it evenly between all of them:
```!SNIPPET
<Form>
  <VBox Height="200">
    <Panel Width="50" Height="*" BackgroundColor="red" />
    <Panel Width="30" Height="30" BackgroundColor="green" />
    <Panel Width="40" Height="*" BackgroundColor="blue" />
  </VBox>
</Form>
```

### Advanced VBox usage

The distribution of components with `unlimited` size works the way that first the desired component size is obtained and only then any remaining space is distributed evenly between all participating components.
In the sample below, each label has different height, but all of them are expanded by the same space:

```!SNIPPET
<Form>
  <VBox Width="80" Height="300">
    <Panel Width="*" Height="*" BackgroundColor="red">
      <Label Text="Hello" Font="Calibri;20" TextColor="white" />
    </Panel>
    <Panel Width="*" Height="*" BackgroundColor="green">
      <Label Text="How are you?" Font="Calibri;20" TextColor="white" />
    </Panel>
    <Panel Width="*" Height="*" BackgroundColor="blue">
      <Label Text="Have a nice day, won't you agree?" Font="Calibri;20" TextColor="white" />
    </Panel>
  </VBox>
</Form>
```