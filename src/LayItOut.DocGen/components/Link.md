The link is a label that allows specifying `Uri` of the link, that would be included and possible to follow when form is rendered as PDF:

```!SNIPPET
<Form>
  <TextBox>
    <Label Text="Hey, you have to visit this" Font="Calibri;20" TextColor="black"/>
    <Link Text="website" Font="Calibri;20" TextColor="blue" Uri="http://google.com"/>
  </TextBox>
</Form>
```
