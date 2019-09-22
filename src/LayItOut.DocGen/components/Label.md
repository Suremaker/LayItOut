The simple label allows to display text as follows:

```!SNIPPET
<Form>
  <Label Text="Hello world!" Font="Calibri;20" TextColor="black"/>
</Form>
```

The `Inline` attribute controls if text can break at the end of line or enforce rendering in one line.
```!SNIPPET
<Form>
  <HBox>
    <Label Text="Hello world!" Width="65" Font="Calibri;20" TextColor="black" />
    <Label Text="Hello world!" Inline="true" Width="65" Font="Calibri;20" TextColor="black" />
  </HBox>
</Form>
```

Please note that text will get clipped when get outside of label boundary.

The `LineHeight` allow to control height of the line, where `1.0` is default font height.
```!SNIPPET
<Form>
  <VBox Width="300">
    <Label LineHeight="1.2" Font="Calibri;20" TextColor="green"
      Text="Hello friend, what's a beautiful day today, don't you agree?" />
    <Label LineHeight="0.8" Font="Calibri;20" TextColor="blue"
      Text="Hello friend, what's a beautiful day today, don't you agree?" />
  </VBox>
</Form>
```

The `TextContinuation` is useful if label is being used within a `TextBox` component. It allows controlling if given label is a continuation of the previous one or a space should be inserted in between.

```!SNIPPET
<Form>
  <TextBox>
    <Label Text="H" Font="Calibri;20" TextColor="green" />
	<Label Text="E" TextContinuation="true" Font="Calibri;20" TextColor="black"/>
	<Label Text="L" TextContinuation="true" Font="Calibri;20" TextColor="red"/>
	<Label Text="L" TextContinuation="true" Font="Calibri;20" TextColor="blue"/>
	<Label Text="O" TextContinuation="true" Font="Calibri;20" TextColor="brown"/>
	<Label Text="friend" TextContinuation="false" Font="Calibri;20" TextColor="black"/>
	<Label Text="!" TextContinuation="true" Font="Calibri;20" TextColor="red"/>
  </TextBox>
</Form>
```