import React, { Component } from 'react';
import { connect } from 'react-redux';
import AddressBlock from './AddressBlock';
import AddressDialog from './AddressDialog';
import { getUI, modifyAddress } from '../../../AC/settingsAddresses';
import Spinner from '../../Spinner';

class SettingAddresses extends Component {
  constructor() {
    super();

    this.state = {
      isDialogOpen: false,
      address: {}
    };

    this.closeDialog = this.closeDialog.bind(this);
    this.openDialog = this.openDialog.bind(this);
    this.changeDataAddress = this.changeDataAddress.bind(this);
  }

  componentDidMount() {
    this.props.getUI();
  }

  openDialog(address) {
    this.setState({
      isDialogOpen: true,
      address
    });
  }

  closeDialog() {
    this.setState({
      isDialogOpen: false
    });
  }

  changeDataAddress(data) {
    this.props.modifyAddress(data);
    this.closeDialog();
  }

  render() {
    const { ui } = this.props;
    const { isDialogOpen, address } = this.state;

    const commonProps = {
      openDialog: this.openDialog,
      closeDialog: this.closeDialog
    };

    const content = Object.keys(ui).length
    ? <div className="settings__block">
        <AddressDialog isDialogOpen={isDialogOpen}
                       closeDialog={this.closeDialog}
                       changeDataAddress={this.changeDataAddress}
                       dialog={ui.dialog}
                       address={address} />
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
  modifyAddress
})(SettingAddresses);

