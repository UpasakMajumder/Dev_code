import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* components */
import Dialog from 'app.dump/Dialog';
/* globals */
import { TAC, LOGIN } from 'app.globals';
/* ac */
import { acceptTaC } from 'app.ac/login';

class TaCDialog extends Component {
  static propTypes = {
    acceptTaC: PropTypes.func.isRequired,
    login: PropTypes.shape({
      credentinals: PropTypes.shape({
        loginEmail: PropTypes.string.isRequired,
        password: PropTypes.string.isRequired
      }).isRequired,
      checkTaC: PropTypes.shape({
        showTaC: PropTypes.bool.isRequired,
        url: PropTypes.string.isRequired
      }).isRequired
    }).isRequired
  };

  submit = () => {
    const { acceptTaCUrl } = LOGIN;
    const { loginEmail, password } = this.props.login.credentinals;
    this.props.acceptTaC(acceptTaCUrl, { loginEmail, password });
  };

  render() {
    const { showTaC, url } = this.props.login.checkTaC;
    const { title, submitTexT } = TAC;

    if (!showTaC) return null;

    const body = (
      <iframe src={url} width={900} height={450} />
    );

    const footer = (
      <div className="btn-group btn-group--right">
        <button
          onClick={this.submit}
          type="button"
          className="btn-action"
        >
          {submitTexT}
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
  const { login } = state;
  return { login };
}, {
  acceptTaC
})(TaCDialog);
