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
import { closeTAC, checkTaC } from 'app.ac/tac';

class TaCDialog extends Component {
  static propTypes = {
    show: PropTypes.bool.isRequired,
    redirect: PropTypes.bool.isRequired,
    returnurl: PropTypes.string.isRequired,
    isChecked: PropTypes.bool.isRequired,
    token: PropTypes.string.isRequired,
    // func
    closeTAC: PropTypes.func.isRequired,
    checkTaC: PropTypes.func.isRequired
  };

  componentDidMount() {
    if (TAC.token) {
      this.props.checkTaC({
        url: TAC.checkTaCUrl,
        token: TAC.token,
        redirect: false,
        returnurl: ''
      });
    }
  }

  componentWillReceiveProps(next) {
    if (!this.props.isChecked && next.isChecked && !next.show && next.redirect) {
      location.assign(next.returnurl);
    }
  }

  submit = () => {
    const {
      redirect,
      returnurl,
      token,
      closeTAC
    } = this.props;

    const { acceptTaCUrl } = TAC;

    axios.post(acceptTaCUrl, { token })
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
    const {
      show
    } = this.props;

    if (!show) return null;

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
  checkTaC
})(TaCDialog);
