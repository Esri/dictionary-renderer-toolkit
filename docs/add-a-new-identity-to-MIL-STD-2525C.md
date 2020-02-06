# Add a new identity to MIL-STD-2525C

## Add a paramilitary identity
The example below illustrates how to add new paramilitary identities to the MIL-STD-2525C dictionary. It creates a custom dictionary that is a modified version of the MIL-STD-2525C dictionary with an updated script that defines the new paramilitary identities or affiliations that uses the existing Hostile frames but applies a different color.  

The MIL-STD-2525C dictionary style contains various symbol components that can be combined to form complex symbols. For example, in the style there are individual symbol components such as the frame, main icons, and echelons. Each of these individual symbol components has a key assigned to it. The dictionary script builds the necessary keys of the composite symbol based on the attributes in the data and the conditions within the script. The concatenated keys produced by the script are matched to the individual symbol components that are combined to create the final symbol. For example, to get the frame for a symbol, the values for the coding scheme, identity/affiliation, battle dimension and status are combined and matched to the appropriate frame in the style.  

In this exercise, new identity values added are `X for Known Paramilitary` and `Y for Suspected Paramilitary`.

To add these new identities the dictionary script must be updated to recognize the value for the new identity/affiliation and to define the new color.

## Create a custom dictionary
In order to modify the dictionary for MIL-STD-2525C it is necessary to create a custom dictionary.
1. Download the [Joint Military Symbology MIL-STD-2525C](https://www.arcgis.com/home/item.html?id=96fd0d8bb7214755a45818e57ce74988) style from ArcGIS Online.
2. Open the copy of the mil2525c.stylx in an SQLite database editor.
3. In the meta table, update the `dictionary_name` to the name of the custom dictionary.
4. Make the style file to editable by changing the `readonly` value to `false`.

## Updating script
To modify the script, copy the value of the `dictionary_script` key in the meta table in the .stylx file and paste it into Notepad++. See [Tips for creating custom dictionaries](tips-for-creating-custom-dictionaries.md). To add the new paramilitary identities, the new identity codes must be added to the script and the new color must be added for the frames.  

1. The first place the values must be added is the list of valid identities or affiliation values.  Any identity that is not in this list is considered invalid.  

```
  // validation for feature templates

  if (indexof(['F', 'H', 'N', 'U', 'P', 'S', 'J', 'K', 'A', 'G', 'W', 'D', 'L', 'M', 'X', 'Y'], _affiliation) == -1) _affiliation = 'U';

  ```

2. The second place to add the values is to the mapping for the base affiliation and color affiliation variables.  The new identity / affiliation values need to be added to the list of the decode function for the variables.  

The base affiliation variable is used in the script in places where the frame width is important, for example, the placement of the headquarter staff. All the identities that use the same frame shape are mapped to the base affiliation. Since the shape of the paramilitary symbols is the same as Hostile symbols, the shape can be mapped to Hostile (`H`). An example of this in the MIL-STD-2525C dictionary that is installed in ArcGIS Pro is where the Joker (`J`) identity is mapped to the base affiliation of Friend (`F`). For the Known and Suspect Paramilitary identities add them to the decode list mapping them to `H`.

The color affiliation is used to map identities that use the same color.  Since the paramilitary symbols will have a different color than existing hostile identity the values are also added to the decode list for color affiliation variable. The Known and Suspect Paramilitary symbols will have the same color so they should both be mapped to `X`, since the paramilitary symbols will have a different color than existing hostile identity.

```
// utilities

var _base_affiliation = decode(_affiliation, 'F', 'F', 'A', 'F', 'M', 'F', 'D', 'F', 'J', 'F', 'K', 'F', 'N', 'N', 'L', 'N', 'H', 'H', 'S', 'H', 'X', 'H', 'Y', 'H', 'U');
var _color_affiliation = decode(_affiliation, 'F', 'F', 'A', 'F', 'M', 'F', 'D', 'F', 'J', 'H', 'K', 'H', 'N', 'N', 'L', 'N', 'H', 'H', 'S', 'H', 'X', 'X', 'Y', 'X', 'U');

```

3. Once the color affiliation is established the `X` and `Y` codes are no longer needed, so you can substitute those for the hostile codes and use the existing frames. Since the only difference is the color you can reuse the existing frames and do not need to duplicate the symbols. The `_affiliation` value is the value that is used when composing the frame symbol key. The following can be added below the utilities section.

```
//Paramilitary affiliation, after this line in the script it is ok to forget about the original Paramilitary values of 'X' and 'Y'

if (_affiliation == 'X')
  _affiliation = 'F';
if (_affiliation == 'Y')
  _affiliation = 'D';

  ```

4. The color can be set for the paramilitary identity by adding it to the argument for the frame fill colors.  A value for dark, medium and light (default) colors should be added.

```
// frame fill color

  var _fill_color;
  if (_is_civilian)
    _fill_color = decode($config.colors, 'DARK', '#500050', 'MEDIUM', '#800080', '#FFA1FF'); // civilian
  else
    _fill_color = decode(_color_affiliation,
        'F', decode($config.colors, 'DARK', '#006B8C', 'MEDIUM', '#00A8DC', '#80E0FF'), // friendly
        'N', decode($config.colors, 'DARK', '#00A000', 'MEDIUM', '#00E200', '#AAFFAA'), // neutral
        'H', decode($config.colors, 'DARK', '#C80000', 'MEDIUM', '#FF3031', '#FF8080'), // hostile
 'X', decode($config.colors, 'DARK', '#9F4B6B', 'MEDIUM', '#BC587E', '#E37DA4'), // paramilitary
        decode($config.colors, 'DARK', '#E1DC00', 'MEDIUM', '#FFFF00', '#FFFF80')); // unknown

```

Information on Arcade functions is available at https://developers.arcgis.com/arcade/function-reference/

## Verifying syntax
Once the script has been updated it is good practice to verify the syntax of the script before adding it back into the dictionary.  The syntax of the script can be verified online using the Arcade playground.
1. Go to https://developers.arcgis.com/arcade/playground/
2. Copy the text contained in [mil2525c_b2_app6b_arcade_vars.json](../variable_declarations/mil2525c_b2_app6b_arcade_vars.json) file into the expression window. These are values that you can change to match the symbols you are adding. The values are used for the string that is returned in the results.
3. Copy the text from the edited dictionary script. Paste below the text from `mil2525c_b2_app6b_arcade_vars`.
4. Click Test. If there is a syntax error, the line with the error is reported in the results. If there are no errors the results show a string of the values returned.
Once the script has been verified, you can add it into the .stylx `meta` table.
