import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
/* components */
import Spinner from 'app.dump/Spinner';
/* ac */
import { getUI, addAddress, modifyAddress, setDefault, unsetDefault } from 'app.ac/settingsAddresses';
/* local components */
import AddressBlock from './AddressBlock';
import AddressDialog from './AddressDialog';

class SettingAddresses extends Component {
  state = {
    isDialogOpen: false,
    isModifyingDialog: true,
    address: {}
  };

  static propTypes = {
    ui: PropTypes.shape({
      billing: PropTypes.object,
      shipping: PropTypes.object,
      dialog: PropTypes.object
    }).isRequired
  };

  componentDidMount() {
    this.props.getUI();
  }

  openDialog = (address, isModifying = true) => {
    this.setState({
      isDialogOpen: true,
      isModifyingDialog: isModifying,
      address
    });
  };

  closeDialog = () => {
    this.setState({ isDialogOpen: false });
  };

  changeDataAddress = (data) => {
    const { modifyAddress } = this.props;
    modifyAddress(data);
    this.closeDialog();
  };

  addDataAddress = (data) => {
    const { addAddress } = this.props;
    addAddress(data);
    this.closeDialog();
  };

  render() {
    const { ui, setDefault, unsetDefault } = this.props;
    const { isDialogOpen, isModifyingDialog, address } = this.state;
    const { dialog, billing, shipping } = ui;

    const commonProps = {
      openDialog: this.openDialog,
      dialog
    };

    return Object.keys(ui).length
    ? <div className="settings__block">
        <AddressDialog
          isModifyingDialog={isModifyingDialog}
          closeDialog={this.closeDialog}
          addDataAddress={this.addDataAddress}
          changeDataAddress={this.changeDataAddress}
          dialog={dialog}
          address={address}
          open={isDialogOpen}
        />
        <AddressBlock
          ui={billing}
          setDefault={(id, url) => setDefault('billing', id, url)}
          unsetDefault={(id, url) => unsetDefault('billing', id, url)}
          {...commonProps}
        />
        <AddressBlock
          ui={shipping}
          setDefault={(id, url) => setDefault('shipping', id, url)}
          unsetDefault={(id, url) => unsetDefault('shipping', id, url)}
          {...commonProps}
        />
      </div>
    : <Spinner />;
  }
}

export default connect((state) => {
  const { settingsAddresses } = state;
  return { ui: settingsAddresses };
}, {
  getUI,
  addAddress,
  modifyAddress,
  setDefault,
  unsetDefault
})(SettingAddresses);
