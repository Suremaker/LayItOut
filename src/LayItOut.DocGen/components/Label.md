The simple label allows to display text as follows:

```!SNIPPET
<Form>
	<Label Text="Hello world!" Font="Calibri;20" TextColor="black"/>
</Form>
```

The `Inline` attribute controls if text can break (**default option**) at the end of line or enforce rendering in one line. 
```!SNIPPET
<Form>
	<HBox>
		<Label Text="Hello world!" Width="65" Font="Calibri;20" TextColor="black" />
		<Label Text="Hello world!" Inline="true" Width="65" Font="Calibri;20" TextColor="black" />
	</HBox>
</Form>
```

Please note that text will get clipped when get outside of label boundary.