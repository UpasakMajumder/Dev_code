import React, { Component, PropTypes } from 'react';

import TextInput from '../form/TextInput';
import PasswordInput from '../form/PasswordInput';
import CheckboxInput from '../form/CheckboxInput';

class StyleguideInput extends Component {
  render() {
    const thead = <tr>
                    <th>State</th>
                    <th>Text</th>
                    <th>Password</th>
                    <th>Checkbox</th>
                    <th>Radio</th>
                  </tr>;

    return (
      <table>
        <thead>
        { thead }
        </thead>
        <tbody>
        <tr>
          <td>Normal</td>
          <td><TextInput label="Label" placeholder="Placeholder" /></td>
          <td><PasswordInput label="Label" placeholder="Placeholder" /></td>
          <td><CheckboxInput id="example1" label="Label" type="checkbox"/></td>
          <td><CheckboxInput id="example4" label="Label" type="radio"/></td>
        </tr>

        <tr>
          <td>Error</td>
          <td><TextInput error="Error" label="Label" placeholder="Placeholder" /></td>
          <td><PasswordInput error="Error" label="Label" placeholder="Placeholder" /></td>
          <td><CheckboxInput error="Error" id="example2" label="Label" type="checkbox"/></td>
          <td><CheckboxInput error="Error" id="example5" label="Label" type="radio"/></td>
        </tr>

        <tr>
          <td>Disabled</td>
          <td><TextInput label="Label" disabled placeholder="Placeholder" /></td>
          <td><PasswordInput label="Label" disabled placeholder="Placeholder" /></td>
          <td><CheckboxInput disabled id="example3" label="Label" type="checkbox"/></td>
          <td><CheckboxInput disabled id="example4" label="Label" type="radio"/></td>
        </tr>
        </tbody>
      </table>
    );
  }
}

export default StyleguideInput;
