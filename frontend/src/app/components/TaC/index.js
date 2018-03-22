import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import axios from 'axios';
// /* components */
import Dialog from 'app.dump/Dialog';
// /* globals */
import { TAC } from 'app.globals';
// /* constants */
import { FAILURE } from 'app.consts';
// /* ac */
import { closeTAC, openTaC } from 'app.ac/tac';

class TaCDialog extends Component {
  static propTypes = {
    show: PropTypes.bool.isRequired,
    redirect: PropTypes.bool.isRequired,
    returnurl: PropTypes.string.isRequired,
    // func
    closeTAC: PropTypes.func.isRequired,
    openTaC: PropTypes.func.isRequired
  };

  componentDidMount() {
    if (TAC.accepted === false) { // can be undefined
      this.props.openTaC({
        redirect: false,
        returnurl: ''
      });
    }
  }

  submit = () => {
    const {
      redirect,
      returnurl,
      closeTAC
    } = this.props;

    const { acceptTaCUrl } = TAC;

    axios.get(acceptTaCUrl)
      .then((response) => {
        const { success, errorMessage } = response.data;
        if (success) {
          if (redirect) {
            location.assign(returnurl);
          } else {
            closeTAC();
          }
        } else {
          window.store.dispatch({
            type: FAILURE,
            alert: errorMessage
          });
        }
      })
      .catch((e) => {
        window.store.dispatch({
          type: FAILURE,
          alert: false
        });
      });
  };

  render() {
    const { title, submitText, iframeUrl } = TAC;

    if (!this.props.show) return null;

    const body = (
      <iframe src={iframeUrl} width={900} height={450} />
    );

    const footer = (
      <div className="btn-group btn-group--right">
        <button
          onClick={this.submit}
          type="button"
          className="btn-action"
        >
          {submitText}
        </button>
      </div>
    );

    return (
      <Dialog
        title={title}
        body={body}
        footer={footer}
      />
    );
  }
}

export default connect((state) => {
  const { tac } = state;
  return { ...tac };
}, {
  closeTAC,
  openTaC
})(TaCDialog);
