import React, { Component } from 'react';
import { connect } from 'react-redux';
import AddressBlock from './AddressBlock';
import { getUI, modifyAddress } from '../../../AC/settingsAddresses';
import { openDialog, closeDialog } from '../../../AC/dialog';
import Spinner from '../../Spinner';

class SettingAddresses extends Component {
  componentDidMount() {
    this.props.getUI();
  }

  render() {
    const { ui } = this.props;

    const commonProps = {
      closeDialog: this.props.closeDialog,
      openDialog: this.props.openDialog,
      dialog: ui.dialog,
      modifyAddress: this.props.modifyAddress
    };

    const content = Object.keys(ui).length
    ? <div className="settings__block">
        <div className="settings__item">
          <AddressBlock ui={ui.billing} {...commonProps} />
        </div>
        <div className="settings__item">
          <AddressBlock ui={ui.shipping} {...commonProps} />
        </div>
      </div>
    : <Spinner />;

    return content;
  }
}

export default connect((state) => {
  const { settingsAddresses } = state;
  return { ui: settingsAddresses };
}, {
  getUI,
  openDialog,
  closeDialog,
  modifyAddress
})(SettingAddresses);

