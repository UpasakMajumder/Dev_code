import React, { Component } from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
import { toastr } from 'react-redux-toastr';
import { connect } from 'react-redux';
/* globals */
import { EMAIL_PROOF } from 'app.globals';
import { FAILURE } from 'app.consts';
/* helpers */
import { emailRegExp } from 'app.helpers/regexp';
/* ac */
import toggleModal from 'app.ac/emailProof';
/* components */
import Alert from 'app.dump/Alert';
import Dialog from 'app.dump/Dialog';
import TextInput from 'app.dump/Form/TextInput';
import Textarea from 'app.dump/Form/Textarea';
import Button from 'app.dump/Button';
import EmailConfirmation from '../Checkout/EmailConfirmation';

class EmailProof extends Component {
  state = {
    form: {
      recepientEmail: '',
      senderEmail: '',
      subject: '',
      message: ''
    },
    invalids: [], // { field, message }
    isPending: false
  }

  getInvalids = () => {
    const { required, email } = this.props.dialog.validationMessages;
    const { recepientEmail, senderEmail, subject } = this.state.form;

    const invalids = [];

    // required
    if (!senderEmail) invalids.push({ field: 'senderEmail', message: required });
    if (!subject) invalids.push({ field: 'subject', message: required });
    if (!recepientEmail) invalids.push({ field: 'recepientEmail', message: required });

    // email
    const isRecepientEmailInvalid = recepientEmail.split(',').find(item => !item.trim().match(emailRegExp));
    if (isRecepientEmailInvalid) invalids.push({ field: 'recepientEmail', message: email });

    const isSenderEmailInvalid = senderEmail.split(',').find(item => !item.trim().match(emailRegExp));
    if (isSenderEmailInvalid) invalids.push({ field: 'senderEmail', message: email });

    this.setState({ invalids });
    return invalids;
  };

  getErrorMessage = (field, nestingLevel) => {
    const invalid = this.state.invalids.find(invalid => invalid.field === field);
    if (invalid) return invalid.message;
    return '';
  };

  handleSubmit = async () => {
    const invalids = this.getInvalids();
    if (invalids.length) {
      this.setState({ invalids });
      return;
    }

    const body = {
      ...this.state.form,
      emailProofUrl: this.props.store.emailProofUrl
    };

    try {
      this.setState(prevState => ({ isPending: !prevState.isPending }));

      const response = await axios.post(this.props.submitUrl, body);
      const { success, errorMessage } = response.data;

      if (success) {
        this.setState(prevState => ({ isPending: !prevState.isPending }));
        this.props.toggleModal(false);
        toastr.success(this.props.notificationSuccess.title, this.props.notificationSuccess.text);
      } else {
        this.setState(prevState => ({ isPending: !prevState.isPending }));
        window.store.dispatch({
          type: FAILURE,
          alert: errorMessage
        });
      }
    } catch (e) {
      this.setState(prevState => ({ isPending: !prevState.isPending }));
      window.store.dispatch({
        type: FAILURE,
        alert: false
      });
    }
  };

  closeDialog = () => this.props.toggleModal(false);

  handleChange = (field, value) => {
    this.setState({
      form: {
        ...this.state.form,
        [field]: value
      }
    });

    this.clearInvalid(field);
  };

  clearInvalid = (field) => {
    const fieldString = field.toString();
    this.setState({
      invalids: this.state.invalids.filter(invalid => invalid.field !== fieldString)
    });
  };

  render () {
    const { dialog } = this.props;
    const { form } = this.state;

    const body = (
      <div className="email-proof">
        <Alert
          type="info"
          text={dialog.note}
        />

        <div className="mb-4">
          <TextInput
            label={dialog.recepientEmailLabel}
            error={this.getErrorMessage('recepientEmail')}
            value={form.recepientEmail}
            onChange={e => this.handleChange('recepientEmail', e.target.value)}
          />
        </div>

        <div className="mb-4">
          <TextInput
            label={dialog.senderEmailLabel}
            error={this.getErrorMessage('senderEmail')}
            value={form.senderEmail}
            onChange={e => this.handleChange('senderEmail', e.target.value)}
          />
        </div>

        <div className="mb-4">
          <TextInput
            label={dialog.subjectLabel}
            error={this.getErrorMessage('subject')}
            value={form.subject}
            onChange={e => this.handleChange('subject', e.target.value)}
          />
        </div>

        <div className="mb-2">
          <Textarea
            label={dialog.messageLabel}
            error={this.getErrorMessage('message')}
            value={form.message}
            onChange={e => this.handleChange('message', e.target.value)}
            rows={4}
            isOptional={true}
          />
        </div>

        <a href={this.props.store.emailProofUrl} target="_blank" className="link">{dialog.proofLabel}</a>
      </div>
    );


    const footer = (
      <div className="btn-group btn-group--right">
        <Button
          text={dialog.cancelButtonLabel}
          type="action"
          onClick={this.closeDialog}
          disabled={this.state.isPending}
        />

        <Button
          text={dialog.submitButtonLabel}
          type="action"
          btnClass="btn-action--secondary"
          onClick={this.handleSubmit}
          isLoading={this.state.isPending}
        />
      </div>
    );

    return (
      <Dialog
        closeDialog={this.closeDialog}
        hasCloseBtn={true}
        title={dialog.title}
        body={body}
        footer={footer}
      />
    );
  }

  static defaultProps = {
    ...EMAIL_PROOF
  };

  static propTypes = {
    // config
    submitUrl: PropTypes.string.isRequired,
    notificationSuccess: PropTypes.shape({
      title: PropTypes.string.isRequired,
      text: PropTypes.string.isRequired
    }).isRequired,
    dialog: PropTypes.shape({
      title: PropTypes.string.isRequired,
      recepientEmailLabel: PropTypes.string.isRequired,
      recepientEmailTooltipAdd: PropTypes.string.isRequired,
      recepientEmailTooltipRemove: PropTypes.string.isRequired,
      senderEmailLabel: PropTypes.string.isRequired,
      subjectLabel: PropTypes.string.isRequired,
      messageLabel: PropTypes.string.isRequired,
      proofLabel: PropTypes.string.isRequired,
      submitButtonLabel: PropTypes.string.isRequired,
      cancelButtonLabel: PropTypes.string.isRequired,
      note: PropTypes.string.isRequired,
      validationMessages: PropTypes.shape({
        required: PropTypes.string.isRequired,
        email: PropTypes.string.isRequired
      }).isRequired
    }).isRequired,
    // ac
    toggleModal: PropTypes.func.isRequired,
    // store
    store: PropTypes.shape({
      emailProofUrl: PropTypes.string.isRequired
    }).isRequired
  };
}

export default connect((state) => {
  const { emailProof } = state;
  return { store: emailProof };
}, {
  toggleModal
})(EmailProof);
