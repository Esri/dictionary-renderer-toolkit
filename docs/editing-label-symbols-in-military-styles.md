# Editing Label Symbols in Military Styles

## Editing Label Symbols

This example shows how to modify the label symbols in the MIL-STD-2525D dictionary. This is done by creating a custom dictionary that is a modified version of the MIL-STD-2525D dictionary where the label symbols have been modified in the stylx.  It should be noted that labels in the military styles are modeled as point, line or polygon symbols containing markers with embedded text symbols in the markers.  These are different from the labels in the label style classes that are used in the traditional labeling on maps in ArcGIS Pro.  This is done because the placement of these military labels is not dynamic.

This example is with MIL-STD-2525D but can be done with any of the military symbology dictionaries.

## Create a custom dictionary

To modify the dictionary for MIL-STD-2525D you must first create a custom dictionary.
1. Download the [Joint Military Symbology MIL-STD-2525D](https://www.arcgis.com/home/item.html?id=44b781991d194dd8bc423e642c1932c5) file from ArcGIS Online.
2. Open the copy of the mil2525d.stylx in a SQLite database editor.
3. In the `meta` table, update the `dictionary_name` to the name of the custom dictionary.
4. Make the style file editable by changing the `readonly` value to `false`.

## Find the label symbols

To begin you will need to find all the label symbols in the stylx file.
1. _Add the custom dictionary to ArcGIS Pro:_ In ArcGIS Pro, on the Insert tab, in the Styles group, click Add and click Add Style. Browse to the custom dictionary style. If you are using ArcGIS Pro 3.0 or higher, select yes when asked if you want to upgrade the style.

2. _Open the custom dictionary style:_ On the View tab, in the Windows group, click Catalog View to open a Catalog view. In the Catalog view, expand the Styles folder and double click the custom dictionary to open it.

3. _Locate the label symbols:_ With the custom dictionary open in the Catalog View, enter `label` in the Search box to find all the label symbols. By default the style will open to the point symbols; to search for the line or polygon labels you need to switch to those items in the catalog view. Under Manage on the Styles tab, in the Organize group, select Line or Polygon symbol from the Show drop down.

## Edit the basic symbol properties for multiple symbols

Properties that are available on the symbol tab can be edited at one time for all of the symbols that are part of a selection.  Changes to the properties on the symbol tab will affect all symbol layers in the symbol and will apply to all relevant properties in the symbol layer.  For example, changing the size will change the size of all symbol layers as well as adjust other properties related to size, such as offsets to maintain ratios.  Color changes will be applied to all unlocked colors.

In this case we will change the color for the selected point labels

1. _Select multiple symbols:_ Once you have searched for the label symbols select the symbols you want to modify using SHIFT+Click or CTRL+Click.

2. _Change color of label:_ With multiple symbols selected, in the Details pane click on the Properties tab to open the symbol editor.  Under the Appearance heading, click on the color picker and change the color from black to blue.  Click Apply.

3. _View changes in the symbol preview:_ The changes to the individual symbols can be seen in the symbol preview.  Click the arrows on the side of the preview to scroll through the selected symbols.

## Edit the layer properties for symbol layers

Additional properties are available at the symbol layer level.  Changing properties at this level only applies to the symbol layer and does not propagate to any other symbol properties.  For markers containing text some of those properties include the font, the vertical/horizontal alignment and the text string.

In this example we will change the font.

1. _Select symbol to edit:_ Click on a single symbol to edit. The layer properties can only be edited for a single symbol.

2. _Change font for label:_ In the symbol editor, select the Layers tab (![symbol layers icon](images/symbollayers.png)), under the Appearance heading click the Font drop down list and select a new font such as Tahoma.  Click Apply.

3. _Repeat for all symbol layers_

## Edit properties for embedded text symbol

In order to modify some properties it is necessary to edit the text symbol that is embedded in the marker symbol.  Editing the text symbol allows you to modify additional text properties such as the font style, the text case, or the halo that is around the glyphs. 

In this example we will modify the text case and the halo that is on the embedded text symbol.

1. _Select symbol to edit:_ Click on a single symbol to edit.

2. _Format embedded text symbol:_ In the symbol editor, select the Layers tab (![symbol layers icon](images/symbollayers.png)), under the Appearance heading click the Shape Text Symbol drop down and select Format Text Symbol.

3. _Change the text case:_ Under the Appearance heading, click the drop down for Text Case and select Normal to change it from using all upper case to using the casing that is in the text string provided.

4. _Remove the halo:_ Under the Halo heading, from the Halo Symbol drop down select the No Symbol option to remove the halo symbol.  Alternatively, if you want to keep the symbol and just temporarily keep it from drawing you can change the halo size to 0.
