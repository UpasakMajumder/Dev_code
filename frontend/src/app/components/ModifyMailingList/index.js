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
/* AC */
import { initUI } from 'app.ac/modifyMailingList';
/* local components */
import MailingTable from './MailingTable';

class ModifyMailingList extends Component {
  static propTypes = {
    initUI: PropTypes.func.isRequired,
    uiFail: PropTypes.bool.isRequired,
    ui: PropTypes.shape({
      errorList: PropTypes.shape({
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
      successList: PropTypes.shape({
        header: PropTypes.string.isRequired,
        btns: PropTypes.shape({
          use: PropTypes.string.isRequired
        }).isRequired
      }).isRequired
    })
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

  render() {
    const { uiFail, ui } = this.props;
    if (!ui && !uiFail) return <Spinner/>;
    if (uiFail) return null;

    const { errorList, successList } = ui;
    const { use } = successList.btns;

    if (!errorList.items) {
      consoleException('No found items in errorList');
      return null;
    }

    let errorContainer = null;
    let btnCorrectErrors = null;

    if (errorList.items.length) {
      const { reupload, correct } = errorList.btns;

      errorContainer = (
        <div className="processed-list__table-block">
          <div className="processed-list__table-heading processed-list__table-heading--error">
            <h3>{errorList.header}</h3>
            <div className="btn-group btn-group--right">
              <a className="btn-action btn-action--secondary" href={reupload.url}>{reupload.text}</a>
              <Button text={correct} type="action" btnClass="btn-action--secondary"/>
            </div>
          </div>

          <div className="processed-list__table-inner">
            <MailingTable items={errorList.items}/>
            <span className="processed-list__table-helper">
              {errorList.tip}
              <SVG name="info-arrow" className="help-arrow"/>
            </span>
          </div>
        </div>
      );

      btnCorrectErrors = (
        <div className="btn-group btn-group--right">
          <Button text={use} type="action" btnClass="btn-action--secondary"/>
        </div>
      );
    }

    return (
      <div className="processed-list">
        {errorContainer}

        <div className="processed-list__table-block">
          <div className="processed-list__table-heading processed-list__table-heading--success">
            <h3>{successList.header}</h3>
            {btnCorrectErrors}
          </div>

          <MailingTable items={successList.items}/>
        </div>
      </div>
    );
  }
}

export default connect((state) => {
  const { uiFail, ui } = state.modifyMailingList;
  return { uiFail, ui };
}, {
  initUI
})(ModifyMailingList);
