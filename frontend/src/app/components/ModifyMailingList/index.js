// @flow
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
/* AC */
import { initUI, useCorrect } from 'app.ac/modifyMailingList';
/* local components */
import MailingTable from './MailingTable';

class ModifyMailingList extends Component {
  static propTypes = {
    initUI: PropTypes.func.isRequired,
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
    formInfo: PropTypes.object,
    successUI: PropTypes.shape({
      header: PropTypes.string.isRequired,
      btns: PropTypes.shape({
        use: PropTypes.string.isRequired
      }).isRequired
    }),
    errorList: PropTypes.array,
    successList: PropTypes.array
  };

  componentDidMount() {
    const { initUI } = this.props;
    initUI();
  }

  componentWillReceiveProps(nextProps) { // eslint-disable-line class-methods-use-this
    if (nextProps.uiFail) {
      alert('No UI, try to reload the page'); // eslint-disable-line no-alert
    }
  }

  handleUseCorrect = () => {
    const { useCorrect } = this.props;
    const { containerId } = getSearchObj();
    useCorrect(containerId);
  };

  render() {
    const { uiFail, errorUI, successUI, errorList, successList, formInfo } = this.props;
    if (uiFail) return null;

    let errorContainer = null;
    let successContainer = null;
    let btnCorrectErrors = null;

    if (errorList) {
      const { reupload, correct } = errorUI.btns;
      const { use } = successUI.btns;

      errorContainer = (
        <div className="processed-list__table-block">
          <div className="processed-list__table-heading processed-list__table-heading--error">
            <h3>{errorUI.header}</h3>
            <div className="btn-group btn-group--right">
              <a className="btn-action btn-action--secondary" href={reupload.url}>{reupload.text}</a>
              <Button text={correct} type="action" btnClass="btn-action--secondary"/>
            </div>
          </div>

          <div className="processed-list__table-inner">
            <MailingTable items={errorList}/>
            <span className="processed-list__table-helper">
              {errorUI.tip}
              <SVG name="info-arrow" className="help-arrow"/>
            </span>
          </div>
        </div>
      );

      btnCorrectErrors = (
        <div className="btn-group btn-group--right">
          <Button text={use} onClick={() => this.handleUseCorrect()} type="action" btnClass="btn-action--secondary"/>
        </div>
      );
    }

    if (successList) {
      successContainer = (
        <div className="processed-list__table-block">
          <div className="processed-list__table-heading processed-list__table-heading--success">
            <h3>{successUI.header}</h3>
            {btnCorrectErrors}
          </div>

          <MailingTable items={successList}/>
        </div>
      );
    }

    return (
      <div className="processed-list">
        {errorContainer}
        {successContainer}
      </div>
    );
  }
}

export default connect((state) => {
  const { uiFail, errorUI, successUI, errorList, successList, formInfo } = state.modifyMailingList;
  return { errorUI, successUI, errorList, successList, uiFail, formInfo };
}, {
  initUI,
  useCorrect
})(ModifyMailingList);
