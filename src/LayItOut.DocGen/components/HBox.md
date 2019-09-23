### Basic HBox usage

HBox lays out the inner components horizontally from left to right:

```!SNIPPET
<Form>
  <HBox>
    <Panel Width="50" Height="50" BackgroundColor="red" />
    <Panel Width="30" Height="30" BackgroundColor="green" />
    <Panel Width="40" Height="40" BackgroundColor="blue" />
  </HBox>
</Form>
```

By default, the HBox will try to expand to accommodate it's all components. If it's smaller than required size, it will clip the components from the bottom-right:
```!SNIPPET
<Form>
  <HBox Width="85" Height="35">
    <Panel Width="50" Height="50" BackgroundColor="red" />
    <Panel Width="30" Height="30" BackgroundColor="green" />
    <Panel Width="40" Height="40" BackgroundColor="blue" />
  </HBox>
</Form>
```

If it is bigger than required size, it will lay the components as is:
```!SNIPPET
<Form>
  <HBox Width="200" Height="80">
    <Panel Width="50" Height="50" BackgroundColor="red" />
    <Panel Width="30" Height="30" BackgroundColor="green" />
    <Panel Width="40" Height="40" BackgroundColor="blue" />
  </HBox>
</Form>
```

It honors components with `unlimited size`, distributing it evenly between all of them:
```!SNIPPET
<Form>
  <HBox Width="200" Height="80">
    <Panel Width="*" Height="50" BackgroundColor="red" />
    <Panel Width="30" Height="30" BackgroundColor="green" />
    <Panel Width="*" Height="40" BackgroundColor="blue" />
  </HBox>
</Form>
```

### Advanced HBox usage

The distribution of components with `unlimited` size works the way that first the desired component size is obtained and only then any remaining space is distributed evenly between all participating components.
In the sample below, each label has different width, but all of them are expanded by the same space:

```!SNIPPET
<Form>
  <HBox Width="400">
    <Panel Width="*" BackgroundColor="red">
      <Label Text="Hello" Font="Calibri;20" TextColor="white" />
    </Panel>
    <Panel Width="*" BackgroundColor="green">
      <Label Text="How are you?" Font="Calibri;20" TextColor="white" />
    </Panel>
    <Panel Width="*" BackgroundColor="blue">
      <Label Text="Have a nice day!" Font="Calibri;20" TextColor="white" />
    </Panel>
  </HBox>
</Form>
```