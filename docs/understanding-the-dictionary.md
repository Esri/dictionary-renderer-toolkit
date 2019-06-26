# Understanding the Dictionary

## What is a Dictionary?

A dictionary is a mobile style file (.stylx) that contains instructions that defines how symbols are constructed based on multiple attributes. The instructions are the combination of the dictionary configuration file and the dictionary script which are stored within the meta table of the style file. A dictionary allows you to author individual parts of a symbol that can be combined by the instructions allowing for the creation of many symbol permutations without having to author each complete symbol. This approach is useful when symbol specifications lead to many symbol permutations that would be inappropriate for unique value symbology.

## Dictionary Style

For a dictionary to be used in Runtime the style file must be a mobile style file. The style should contain all the individual component symbols that will be combined to create the final symbols. These component symbols are identified by the unique Key assigned to each item in the style.

For example, with the MIL-STD-2525D dictionary, which is included in ArcGIS Pro, the style contains individual component symbols for the frames, main icons, modifiers, echelons, indicators, etc. which are combined to create a specific final symbol.

*Component symbols in a style*

![An image of component symbols in a style](images/componentsymbolsinastyle.png)

*Final symbol*

![Final symbol](images/finalsymbol.png)

The dictionary script defines the keys for the individual component symbols. These keys may be a combination of variables, attributes, string, etc. In the example above, the frame key, which returns the blue rectangle, is built by combining the values for context, identity, symbol set and status.

Frame key example code:
`concatenate([_context, '_', $identity, _symbolset, '_', _status])`


Mobile style files that are used as dictionaries must contain the following fields in the `meta` table of the .stylx file:

**_dictionary_configuration_**

This is the configuration file that contains properties that are shown in the dictionary symbology pane in ArcGIS Pro. This contains the options for the configuration fields, such as the ability to turn the frame on and off. It also has a list of all the mappable symbol and text properties that are shown in the symbology pane. This can be copied from the `meta` table in the .stylx file and edited in any text editor.

**_dictionary_script_**

The script is an arcade script that contains all the instructions on how to use attribute values to build symbol keys. Those keys are used to lookup symbols in the dictionary style. The collection of symbols identified by this process create the final dictionary symbol. This can be copied from the meta data table in the .stylx file and edited in any text editor.

**_dictionary_name_**

This is the name that will show in the dictionary symbology pane in ArcGIS Pro.

**_dictionary_version_**

This is the version of the dictionary. The minimum version for a custom dictionary is `2.0.0`

**_arcade_version_**

The arcade version indicates the minimum version of arcaded need to consume the script. The minimum arcade version for a custom dictionary is `1.5.0`.

## Dictionary Configuration
This json object defines attributes which will appear in the UI to allow mapping to database attributes. Those attributes will be available as variables in the script, prefixed by the `$` character.

### Configuration
This section lists options that are not feature dependent. Only text values are supported.

Example:
```
declaration
{
    "name": "icon",
    "value": "ON",
    "domain": ["ON", "OFF"],
    "info": "indicates if the icon is rendered"
}
```

Usage in the script:

`var _show_icon = $icon != 'OFF';`

### Symbol
The symbol attributes are used to control how to build symbol keys. This is just an array of strings. The order in the array defines the order in the UI. They are used in the script with the `$` character prefix.

### Text
The text attributes are only used by the dictionary labels. They are not available as variables in the script.

*Labels are modeled as text symbols in markers.*

![Labels are modeled as text symbols in markers](images/labelmarkers.png)

There is a TextString property which can be used to contain an expression. If `uniquedesignation` is a string listed in the text attributes, you can use `PL [uniquedesignation]` as the expression. If the value mapped to `uniquedesignation` is `ABC`, the expression will be evaluated as `PL ABC` and `PL ABC` is the text that will show on the map.

All the parts of the expression using square brackets are candidate for this substitution mechanism.

There is a custom behavior with the `text` configuration attribute: if the value is `OFF`, attributes are always substituted by an empty string. The example above would show as `PL` in this case.

## Dictionary Script
The dictionary script is an arcade script that returns a string. The string chains together different keys which should match symbols in the dictionary style. All the corresponding symbols are used to compose the final symbol representing the feature.

### Creating Keys
You can use any arcade function to produce the string you want. A key will eventually be used to retrieve a symbol from the style.

Examples:
This uses directly the content of feature attributes (attributes prefixed with `$`)

`var key = concatenate([$symbolset, $symbolentity]);`

This uses a mix of intermediate variables, feature attributes and constant strings.

`var key = concatenate([_context, '_', $identity, _symbolset_frame, _status]);`

### Returning Multiple Keys
Multiple keys are returned as a semi-colon separated string;
Example:
Assuming `_frame_key` and `_icon_key` are keys built in a previous part of the script 

`var keys = _frame_key + ";" + _icon_key;`

### Using Alternate Keys
Some symbol construction may be conditional, or two models may coexist for keys, but only one would be relevant. For instance, in the MIL-STD-2525D dictionary, some icons are independent of the frame shape, while some other are not and thus they have multiple symbols provided in the dictionary. The encoding of the key is either the raw entity number, or the entity number with a suffix representing the frame shape.

The script will attempt to get the generic key, but if it is not found, will use an alternate key. This is achieved by concatenating key with the pipe (`|`) character.

Example:
```
keys += concatenate([
     concatenate([$symbolset, $symbolentity]), // non touching frames
     concatenate([$symbolset, $symbolentity, _affiliation_icon]) // touching frames - we have to provide both since we can't query the existence of the icon
    ], '|'); // use | as the separator
```

### Using Overrides
The value returned by the script can also contain overrides. Think of it as a special key that starts with `po:` (for primitive override).

Primitive override is a way to change a symbol property value differently per feature. Parts of the symbol can be tagged with a primitive name (like an element name in WPF or HTML).
The syntax for a primitive override is `po:<primitive_name>|<property_name>|<value>` which means: for this symbol, replace <property_name> by <value> for all parts that are tagged <primitive_name>.

In the MIL-STD-2525 dictionaries, the frame symbols have built-in primitive names to identify parts that belong to the fill (`frame_fill`) or the outline (`frame_outline`). They can then be colored by overriding the `Color` property.

Example:
```
   if (_show_fill) {
    keys += ';po: frame_fill|Color|';
    keys += _fill_color;
   }
```
Other examples can be found in the MIL-STD-2525 scripts that override offset or color with various other primitive names.


