The following example presents how to combine text having multiple colors, fonts and contains links, while `TextAlignment` allows to control how to layout the text within the line:

```!SNIPPET
<Form>
  <VBox>
    <TextBox Width="250">
      <Label Font="Calibri;30;bold" TextColor="green" Text="Hello!&#10;" />
      <Label Font="Calibri;20" TextColor="black" Text="Hello my dear"/>
      <Label Font="Calibri;20;bold" TextColor="green" Text="friend"/>
      <Label Font="Calibri;20" TextColor="black" TextContinuation="true" Text="." />
      <Label Font="Calibri;20" TextColor="black"
        Text="It's a long time since we spoke. Did you see my new" />
      <Link Font="Calibri;20" TextColor="blue" Text="project"
        Uri="https://github.com/Suremaker/LayItOut"/>
      <Label Font="Calibri;20" TextColor="black" TextContinuation="true" Text="?" />
    </TextBox>
    <TextBox Width="250" TextAlignment="right" >
      <Label Font="Calibri;30;bold" TextColor="green" Text="Hello!&#10;" />
      <Label Font="Calibri;20" TextColor="black" Text="Hello my dear"/>
      <Label Font="Calibri;20;bold" TextColor="green" Text="friend"/>
      <Label Font="Calibri;20" TextColor="black" TextContinuation="true" Text="." />
      <Label Font="Calibri;20" TextColor="black"
        Text="It's a long time since we spoke. Did you see my new" />
      <Link Font="Calibri;20" TextColor="blue" Text="project"
        Uri="https://github.com/Suremaker/LayItOut"/>
      <Label Font="Calibri;20" TextColor="black" TextContinuation="true" Text="?" />
    </TextBox>
    <TextBox Width="250" TextAlignment="justify" >
      <Label Font="Calibri;30;bold" TextColor="green" Text="Hello!&#10;" />
      <Label Font="Calibri;20" TextColor="black" Text="Hello my dear"/>
      <Label Font="Calibri;20;bold" TextColor="green" Text="friend"/>
      <Label Font="Calibri;20" TextColor="black" TextContinuation="true" Text="." />
      <Label Font="Calibri;20" TextColor="black"
        Text="It's a long time since we spoke. Did you see my new" />
      <Link Font="Calibri;20" TextColor="blue" Text="project"
        Uri="https://github.com/Suremaker/LayItOut"/>
      <Label Font="Calibri;20" TextColor="black" TextContinuation="true" Text="?" />
    </TextBox>
    <TextBox Width="250" TextAlignment="center" >
      <Label Font="Calibri;30;bold" TextColor="green" Text="Hello!&#10;" />
      <Label Font="Calibri;20" TextColor="black" Text="Hello my dear"/>
      <Label Font="Calibri;20;bold" TextColor="green" Text="friend"/>
      <Label Font="Calibri;20" TextColor="black" TextContinuation="true" Text="." />
      <Label Font="Calibri;20" TextColor="black"
        Text="It's a long time since we spoke. Did you see my new" />
      <Link Font="Calibri;20" TextColor="blue" Text="project"
        Uri="https://github.com/Suremaker/LayItOut"/>
      <Label Font="Calibri;20" TextColor="black" TextContinuation="true" Text="?" />
    </TextBox>
  </VBox>
</Form>
```