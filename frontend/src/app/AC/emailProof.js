import { EMAIL_PROOF_TOGGLE } from 'app.consts';

export default (show, emailProofUrl) => {
  return {
    type: EMAIL_PROOF_TOGGLE,
    payload: {
      show,
      emailProofUrl
    }
  };
};
