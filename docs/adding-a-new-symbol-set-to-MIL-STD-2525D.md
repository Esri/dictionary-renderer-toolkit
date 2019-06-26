# Adding a New Symbol Set to MIL-STD-2525D

## Adding a new symbol set

The example below illustrates how to add a new symbol set to the MIL-STD-2525D dictionary. This can be achieved by creating a custom dictionary that is a modified version of the MIL-STD-2525D dictionary with a new frame added to the .stylx, and an updated script defining a new symbol set that uses that frame. 

The new symbol set will have a value of `32`.

## Create a Custom Dictionary

In order to modify the dictionary for MIL-STD-2525D it is necessary to create a custom dictionary.
1. Copy the mil2525d.stylx file from `…/Resources/Dictionaries/mil2525d` in the install location and paste into the location the custom dictionary will be stored.
2. Open the copy of the mil2525d.stylx in an SQLite database editor.
3. In the `meta` table, update the `dictionary_name` to the name of the custom dictionary.
4. Set the style file to editable by changing the `readonly` value to `false`.

## Add new frame to style

The new symbol set will have a different shaped frame then the existing symbol sets so a new symbol needs to be added to the style.
1. _Add the custom dictionary to ArcGIS Pro_. On the ribbon in Pro select Add > Add Style from the style group on the Insert tab and navigate to the custom dictionary style.

2. _Create a new editing style_. Since the dictionary style is very large it is better to create and modify symbols in a separate editing style and then merge them back into the dictionary style once they are finalized. To add the new style select **New > New Mobile Style** from the style group on the Insert tab and create the new style.

3. _Open the custom dictionary_ in the catalog view. Double click on Styles in the Catalog window, then double click on the custom dictionary. 

4. _Create new point symbol_. The easiest starting point is to duplicate and existing frame symbol and then modify it. By doing this you may avoid having to alter numerous properties such as size, anchor points, meta data, etc.

 a. _Locate the symbol you want to copy in the dictionary_. Search for `Frame`, this will find all the Frame symbols. Select the frame you would like to copy. You can select multiple symbols to copy.
 
 b. _Duplicate symbol in editing style_. Right click on the symbol and select Copy. Go back to Styles and double click on the new editing style that was created. Paste the symbol into the editing style. 
 
 c. _Select Ok for message_. When you paste the symbol, a message is shown indicating that you are adding to a mobile style and the symbol may be downgraded. Press OK. 

5. _Update the meta data for the new point symbol_. Select the new symbol and select the Description tab at the bottom of the symbol editor. 

   a. Update the name, category and tags for the new frame as appropriate. Category should have Frame in the name to ensure that the primitive overrides that allow for the colors to be changed can be set by the add-in.
   
   b. Update the key for the new frame. The format for the symbol `Key` is `Context_IdentitySymbolSet_Status`. Status is either `0` for present or `1` for planned. For example, if you are adding a new frame that will be for symbol set `32` the key could be `0_432_0` for a new neutral, present frame. 

6. _Modify frame to new shape_. In the symbol editor select Properties. In Properties tab select Layers (![symbol layers icon](images/symbollayers.png)
). Import a new frame by selecting File in the Appearance section and navigating to the desired SVG/EMF. Once selected click apply.

## Update Script

Once the new symbol has been added, the new symbol set needs to be added to the script. To modify the script, copy it from the meta table in the .stylx file and open the dictionary script file in Notepad++. See [Tips for Creating Custom Dictionaries](tips-for-creating-custom-dictionaries.md).

_Update script for new frame_. To add a new frame, a new symbol set need to be added to the script. This is added in two places. 

1. The first is the list of valid symbol set values. Any symbol set that is not in this list will be considered invalid. 

```
// validity of critical attributes

var _invalid_symbolset = indexof(['00', '01', '02', '05', '06', '10', '11', '15', '20', '25', '30', '32', '35', '36', '40', '45', '46', '47', '50', '51', '52', '53', '54', '60'], $symbolset) == -1;
var _invalid_identity = indexof(['0', '1', '2', '3', '4', '5', '6'], $identity) == -1;

```

2. The second is to add it to the frame symbols. The new symbol set value needs to be added to the list of the decode function. The decode function matches the symbol set value to a frame value. The decode function is used because some symbol sets have the same frame shapes. To minimize the number of symbols in the dictionary, symbol sets are mapped to the frame from the primary symbol set. For example, symbol sets `Air (01)` and `Air Missile (02)` are mapped to the `Air frame`.

```
// frame symbol

else {

 keys = '';

 // map symbolset for frame icons
 var _symbolset_frame = decode($symbolset, '01', '01', '02', '01', '05', '05', '06', '05', '10', '10', '11', '10', '15', '30', '20', '20', '30', '30', '32', '32', '35', '35', '36', '35', '40', '40', '50', '05', '51', '01', '52', '30', '53', '30', '54', '35', '60', '30', '00');
 
 ```

Information on Arcade function is available at https://developers.arcgis.com/arcade/function-reference/ 

## Verifying Syntax
Once the script has been updated it is good practice to verify the syntax of the script before adding it back into the dictionary. The syntax of the script can be verified online using the Arcade playground.
1. Go to https://developers.arcgis.com/arcade/playground/
2. Copy the text from [mil2525d_app6d_arcade_vars.json](../variable_declarations/mil2525d_app6d_arcade_vars.json) into the expression window. These are values that you can change to match the symbols you are adding. The values will be used for the string that is returned in the results.
3. Copy in the text from the edited dictionary script. Paste below the text from `mil2525d_app6d_arcade_vars`.
4. Press Test. If there is a syntax error, the line with the error will be reported in the results. If there are no errors the results will show a string of the values returned. You can change the values at the top to test what different keys are returned.

## Replace script and update dictionary
Once the script is verified in can be added back into the .stylx file `meta` table. To have the colors updated as expected for the new symbol set, the primitive override names need to be set on the symbol. This can be set using the Set Color Overrides for Military Symbols add-in. This should be done on the editing style and then the new symbol can be merged into the dictionary style.
1. To add the script back into the dictionary, copy and paste the script into `dictionary_script` in the `meta` table of the .stylx.
2. Update the color overrides. Overrides are used in the dictionary to change the color of the frames when they are civilian or when the color configuration is set to medium or dark. For these to work a primitive override name needs to be applied to the frame. This is not currently exposed in the Pro UI but may be in the future. This should be done on the editing style before merging the new frame into the dictionary style.

  a. Add the [Set Color Overrides for Military Symbols add-in](../Add-Ins/Set_Color_Overrides_for_Military_Symbols) to ArcGIS Pro.
 
 b. In Pro open the add-in from the Add-In ribbon
 
 c. Navigate to the editing style and click Set Color Override, the editing style will be added to the project when complete.
3. Add the new frame to the dictionary style. While the symbols in the editing style can be copied and pasted back into the dictionary manually, this may change the keys if they already exist. It is important that the keys remain the same. In order to preserve the key on the new symbol the [Merge Style add-in](../Add-Ins/Merge_Styles) can be used.

 a. Add the Merge Style add-in to ArcGIS Pro
 
 b. In Pro open the add-in from the Add-In ribbon
 
 c. For the style to merge into browse to the custom dictionary style
 
 d. For the style to merge browse to the editing style
 
 e. Check Replace keys
 
 f. Click Merge Style. Ensure that symbols are being merged into the correct style and click ok. A message will appear when complete showing how many symbols were copied.
 
Note: This workflow can be repeated for all the various versions of the frame. For example, planned or exercise.

