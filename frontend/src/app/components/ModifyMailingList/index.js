import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
/* components */
import Button from 'app.dump/Button';
import Spinner from 'app.dump/Spinner';
import SVG from 'app.dump/SVG';
/* helpers */
import { consoleException } from 'app.helpers/io';
import { getSearchObj } from 'app.helpers/location';
import { filterByLessNumber, compareArrays } from 'app.helpers/array';
import { findInequals } from 'app.helpers/object';
/* AC */
import { initUI, useCorrect, reprocessAddresses, validationErrors } from 'app.ac/modifyMailingList';
/* local components */
import MailingTable from './MailingTable';
import MailingDialog from './MailingDialog';

class ModifyMailingList extends Component {
  static propTypes = {
    emptyFields: PropTypes.object.isRequired,
    containerId: PropTypes.string,
    canReprocess: PropTypes.bool.isRequired,
    initUI: PropTypes.func.isRequired,
    useCorrect: PropTypes.func.isRequired,
    reprocessAddresses: PropTypes.func.isRequired,
    validationErrors: PropTypes.func.isRequired,
    uiFail: PropTypes.bool.isRequired,
    errorUI: PropTypes.shape({
      header: PropTypes.string.isRequired,
      btns: PropTypes.shape({
        reupload: PropTypes.shape({
          url: PropTypes.string.isRequired,
          text: PropTypes.string.isRequired
        }).isRequired,
        correct: PropTypes.string.isRequired
      }).isRequired
    }),
    formInfo: PropTypes.shape({
      confirmChanges: PropTypes.shape({
        redirect: PropTypes.string.isRequired,
        request: PropTypes.string.isRequired
      }).isRequired
    }),
    successUI: PropTypes.shape({
      header: PropTypes.string.isRequired,
      btns: PropTypes.shape({
        use: PropTypes.shape({
          text: PropTypes.string.isRequired,
          url: PropTypes.string.isRequired
        }).isRequired
      }).isRequired
    }),
    errorList: PropTypes.array,
    filteredErrorList: PropTypes.array,
    filteredSuccessList: PropTypes.array
  };

  state = {
    isDialogShown: false
  };

  componentDidMount() {
    const { initUI } = this.props;
    const { containerId } = getSearchObj();
    initUI(containerId);
  }

  componentWillReceiveProps(nextProps) { // eslint-disable-line class-methods-use-this
    if (nextProps.uiFail) {
      alert('No UI, try to reload the page'); // eslint-disable-line no-alert
    }

    if (nextProps.canReprocess) {
      location.href = nextProps.formInfo.confirmChanges.redirect;
    }
  }

  handleUseCorrect = () => {
    const { useCorrect, successUI, containerId } = this.props;
    useCorrect(containerId, successUI.btns.use.url);
  };

  getEmptyFields = (errorList) => {
    const { fields } = this.props.formInfo;
    const emptyFields = {};

    errorList.forEach((errorItem, index) => {
      Object.entries(errorItem).forEach((item) => {
        const key = item[0];
        const value = item[1];

        if (fields[key]) {
          const isRequired = fields[key].required;
          if (isRequired && !value) {
            if (!emptyFields[index]) emptyFields[index] = [];
            emptyFields[index].push(key);
          }
        }
      });
    });

    return emptyFields;
  };

  handleReprocessAddresses = (errorList) => {
    const { containerId, formInfo, reprocessAddresses, validationErrors } = this.props;
    const emptyFields = this.getEmptyFields(errorList);
    validationErrors(emptyFields);
    const getDifferentRows = compareArrays.bind(null, findInequals);
    const differentRows = getDifferentRows(errorList, this.props.errorList);

    if (differentRows.length) {
      if (!Object.keys(emptyFields).length) reprocessAddresses(containerId, formInfo.confirmChanges.request, differentRows);
    } else {
      this.closeDialog();
    }
  };

  openDialog = () => this.setState({ isDialogShown: true });
  closeDialog = () => {
    this.props.validationErrors({});
    this.setState({ isDialogShown: false });
  };

  render() {
    const { isDialogShown } = this.state;
    const { uiFail,
      errorUI,
      successUI,
      errorList,
      formInfo,
      emptyFields,
      filteredErrorList,
      filteredSuccessList
    } = this.props;

    if (uiFail) return null;

    let errorContainer = null;
    let successContainer = null;
    let btnCorrectErrors = null;

    if (filteredErrorList.length) {
      const { reupload, correct } = errorUI.btns;
      const { use } = successUI.btns;

      errorContainer = (
        <div className="processed-list__table-block">
          <div className="processed-list__table-heading processed-list__table-heading--error">
            <h3>{errorUI.header}</h3>
            <div className="btn-group btn-group--right">
              <a className="btn-action btn-action--secondary" href={reupload.url}>{reupload.text}</a>
              <Button text={correct} onClick={this.openDialog} type="action" btnClass="btn-action--secondary"/>
            </div>
          </div>

          <div className="processed-list__table-inner">
            <MailingTable items={filteredErrorList} fields={formInfo.fields}/>
            <span className="processed-list__table-helper">
              {errorUI.tip}
              <SVG name="info-arrow" className="help-arrow"/>
            </span>
          </div>
        </div>
      );

      btnCorrectErrors = (
        <div className="btn-group btn-group--right">
          <Button text={use.text} onClick={() => this.handleUseCorrect()} type="action" btnClass="btn-action--secondary"/>
        </div>
      );
    }

    if (filteredSuccessList.length) {
      successContainer = (
        <div className="processed-list__table-block">
          <div className="processed-list__table-heading processed-list__table-heading--success">
            <h3>{successUI.header}</h3>
            {btnCorrectErrors}
          </div>

          <MailingTable items={filteredSuccessList} fields={formInfo.fields}/>
        </div>
      );
    }

    return (
      <div className="processed-list">
        {errorContainer}
        {successContainer}
        <MailingDialog
          closeDialog={this.closeDialog}
          formInfo={formInfo}
          emptyFields={emptyFields}
          reprocessAddresses={this.handleReprocessAddresses}
          errorList={errorList}
          open={isDialogShown}
        />
      </div>
    );
  }
}

export default connect((state) => {
  const { uiFail, errorUI, successUI, errorList, successList, formInfo, canReprocess, containerId, emptyFields } = state.modifyMailingList;

  const filterByLessFour = filterByLessNumber.bind(null, 4);

  const filteredErrorList = filterByLessFour(errorList);
  const filteredSuccessList = filterByLessFour(successList);

  return { errorUI,
    successUI,
    errorList,
    uiFail,
    formInfo,
    canReprocess,
    containerId,
    emptyFields,
    filteredErrorList,
    filteredSuccessList
  };
}, {
  initUI,
  useCorrect,
  reprocessAddresses,
  validationErrors
})(ModifyMailingList);
