# Add a new symbol set to MIL-STD-2525D

## Add a new symbol set

This example shows how to add a new symbol set to the MIL-STD-2525D dictionary. This is done by creating a custom dictionary that is a modified version of the MIL-STD-2525D dictionary with a new frame added to the .stylx, and an updated script defining a new symbol set that uses that frame.

The new symbol set will have a value of `32`.

## Create a custom dictionary

To modify the dictionary for MIL-STD-2525D you must first create a custom dictionary.
1. Download the [Joint Military Symbology MIL-STD-2525D](https://www.arcgis.com/home/item.html?id=46294aa60b0b47feaca642450127ae12) file from ArcGIS Online.
2. Open the copy of the mil2525d.stylx in an SQLite database editor.
3. In the `meta` table, update the `dictionary_name` to the name of the custom dictionary.
4. Make the style file editable by changing the `readonly` value to `false`.

## Add a new frame symbol to style

The new symbol set will have a differenty shaped frame than the existing symbol set, so a new symbol must be added to the style.
1. _Add the custom dictionary to ArcGIS Pro_. In ArcGIS Pro, on the Insert tab, in the Styles group, click Add and click Add Style.  Browse to the custom dictionary style.

2. _Create a temporary style:_ Because the dictionary style is very large it is better to create and modify symbols in a temporary style and then merge the symbols back into the dictionary style once they are finalized. To create the temporary style, on the Insert tab, in the Styles group, click New and click New Mobile Style.
 <!--- (edie) I was really confused by an editing style. this sounds like it has something to do with feature editing. can we just call it a temp style? --->

3. _Open the custom dictionary style:_ On the View tab, in the Windows group, click Catalog View to open a Catalog view. In the Contents pane of the Catalog view, expand the Styles folder and click the custom dictionary to open it.

4. _Copy a point symbol:_ It is easier to copy an existing frame symbol and modify it than to construct a new one because you can retain common properties such as size, anchor points, and meta data as applicable.

 a. _Locate the symbol to copy in the dictionary:_ In the Contents pane, click the Dictionary style to highlight it. In the Search box, enter `Frame`, to find all the Frame symbols. Select the frame symbol to copy. You can select multiple symbols to copy.

 b. _Copy symbol in the temporary style:_ Right-click the frame symbol and click Copy. In the Contents pane, right-click the temporary style and click Paste. Click OK in the dialog box indicating that the symbol will be downgraded when pasted into the mobile style.

5. _Update the metadata for the new point symbol:_ Select the new symbol and click the Description tab in the Details panel. If necessary, on the View tab, in the Options group, check Show Details Panel.

   a. Update the name, category, and tags for the new frame symbol as appropriate. The Category must include the word 'Frame' to ensure that the primitive overrides for the colors can be set by the add-in.

   b. Update the key for the new frame symbol. The format for the symbol `Key` is `Context_IdentitySymbolSet_Status`. The status is either `0` for present, or `1` for planned. For example, to add a new frame to symbol set `32`, the key could be `0_432_0` for a new neutral, present frame.

6. _Modify the frame to new shape:_ In the Details panel click Properties. On the Layers tab (![symbol layers icon](images/symbollayers.png)), under the Appearance heading, click File. Browse to an SVG or EMF file containing the new shape and click Open.

## Update the script

Once the new symbol has been added, the new symbol set must be added to the script. To modify the script, copy it from the meta table in the .stylx file and open the dictionary script file in Notepad++. See [Tips for creating custom dictionaries](tips-for-creating-custom-dictionaries.md).

_Update script for new frame:_ To add a new frame, a new symbol set must be added to the script in two places.

1. The first place is the list of valid symbol set values. Any symbol set that is not in this list is considered invalid.

```
// validity of critical attributes

var _invalid_symbolset = indexof(['00', '01', '02', '05', '06', '10', '11', '15', '20', '25', '30', '32', '35', '36', '40', '45', '46', '47', '50', '51', '52', '53', '54', '60'], _symbolset) == -1;
var _invalid_identity = indexof(['0', '1', '2', '3', '4', '5', '6'], _identity) == -1;

```

2. The second place to add the symbol set is to the frame symbols. The new symbol set value must be added to the list of the decode function. The decode function matches the symbol set value to a frame value. The decode function is used because some symbol sets have the same frame shapes. To minimize the number of symbols in the dictionary, symbol sets are mapped to the frame from the primary symbol set. For example, symbol sets `Air (01)` and `Air Missile (02)` are mapped to the `Air frame`.

```
// frame symbol

else {

 keys = '';

 // map symbolset for frame icons
 var _symbolset_frame = decode(_symbolset, '01', '01', '02', '01', '05', '05', '06', '05', '10', '10', '11', '10', '15', '30', '20', '20', '30', '30', '32', '32', '35', '35', '36', '35', '40', '40', '50', '05', '51', '01', '52', '30', '53', '30', '54', '35', '60', '30', '00');

 ```

Information on Arcade functions is available at https://developers.arcgis.com/arcade/function-reference/

## Verify syntax
Once the script has been updated it is good practice to verify the syntax of the script before adding it back into the dictionary. The syntax of the script can be verified online using the Arcade playground.
1. Go to https://developers.arcgis.com/arcade/playground/
2. Copy the text from [mil2525d_app6d_arcade_vars.json](../variable_declarations/mil2525d_app6d_arcade_vars.json) into the expression window. These are values that you can change to match the symbols you are adding. The values will be used for the string that is returned in the results.
3. Copy in the text from the edited dictionary script. Paste below the text from `mil2525d_app6d_arcade_vars`.
4. Click Test. If there is a syntax error, the line with the error is reported in the results. If there are no errors the results show a string of the values returned. You can change the values at the top to test what different keys are returned.

## Replace the script and update the dictionary
Once the script is verified in can be added back into the .stylx file `meta` table. To have the colors updated as expected for the new symbol set, the primitive override names must be set on the symbol. This can be set using the Set Color Overrides for Military Symbols add-in. This should be done on the temporary style and then the new symbol can be merged into the dictionary style.
1. To add the script into the dictionary, copy and paste the script into `dictionary_script` in the `meta` table of the .stylx.
2. Update the color overrides. Overrides are used in the dictionary to change the color of the frames when they are civilian or when the color configuration is set to medium or dark. For these to work, a primitive override name must be applied to the frame. This is not currently exposed in the ArcGIS Pro user interface, but may be in the future. This should be done on the editing style before merging the new frame into the dictionary style.

  a. Add the [Set Color Overrides for Military Symbols add-in](../Add-Ins/Set_Color_Overrides_for_Military_Symbols) to ArcGIS Pro. See [Manage Add-ins](https://pro.arcgis.com/en/pro-app/get-started/manage-add-ins.htm) for additional information about using Add-ins in ArcGIS Pro.

 b. In ArcGIS Pro, open the add-in from the Add-In ribbon.

 c. Browse to the temporary style and click Set Color Override. The temporary style is added to the project when the add-in has finished updating the symbols.

3. Add the new frame symbol to the custom dictionary style. Although the symbols in the temporary style can be copied and pasted into the dictionary manually, this may alter existing keys. It is important that the keys remain the same. To preserve the key on the new symbol use the [Merge Style add-in](../Add-Ins/Merge_Styles).

 a. Add the Merge Style add-in to ArcGIS Pro

 b. In ArcGIS Pro, open the add-in from the Add-In ribbon.

 c. Browse to the custom dictionary style to specify the style to merge into.

 d. Browse to the temporary style to specify the style to merge.

 e. Check Replace keys.

 f. Click Merge Style. Ensure that symbols are merged into the correct style and click OK. A message listing the number of symbols copied appears when the merge is complete.

Note: This workflow can be repeated for all the various versions of the frame, for example, planned or exercise.
