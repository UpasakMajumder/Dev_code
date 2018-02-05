import axios from 'axios';
import { SUBMIT_CARD, FETCH, FAILURE, SUCCESS } from 'app.consts';
import { CARD_PAYMENT } from 'app.globals';

const redirectUser = (dispatch, RedirectURL, submissionId) => {
  axios({
    method: 'get',
    url: `${RedirectURL}/${submissionId}`
  }).then((response) => {
    const { data: redirectData } = response;
    if (!redirectData.success) {
      dispatch({
        type: SUBMIT_CARD + FAILURE,
        alert: redirectData.errorMessage
      });
    } else {
      const { redirectionURL } = redirectData.payload;

      dispatch({ type: SUBMIT_CARD + SUCCESS });

      if (redirectionURL) {
        location.assign(redirectionURL);
      } else {
        setTimeout(() => redirectUser(dispatch, RedirectURL, submissionId), 1000);
      }
    }
  }).catch(() => {
    dispatch({ type: SUBMIT_CARD + FAILURE });
  });
};

export default (fields, cardType, submissionId) => {
  const { name, cvc, number, expiry } = fields;

  return async (dispatch) => {
    try {
      const {
        URL3DSi,
        RedirectURL,
        CustomerIdentifier_MerchantCode,
        CustomerIdentifier_LocationCode,
        CustomerIdentifier_CustomerCode,
        TerminalIdentifier_LocationCode,
        TerminalIdentifier_TerminalCode,
        TerminalIdentifier_MerchantCode,
        APCount,
        PTCount,
        DemoURL,
        ResultURL,
        ResponseType
      } = CARD_PAYMENT;

      if (!submissionId) {
        location.assign(DemoURL); // for demo
        return;
      }

      const data = {
        CreditCard_CSCValue: cvc,
        CreditCard_ExpirationMonth: expiry.substr(0, 2),
        CreditCard_ExpirationYear: expiry.substr(2),
        CreditCard_CardType: cardType,
        CreditCard_NameOnCard: name,
        CreditCard_CardAccountNumber: number,
        SubmissionID: submissionId,
        APCount,
        PTCount,
        CustomerIdentifier_MerchantCode,
        CustomerIdentifier_LocationCode,
        CustomerIdentifier_CustomerCode,
        TerminalIdentifier_LocationCode,
        TerminalIdentifier_TerminalCode,
        TerminalIdentifier_MerchantCode,
        ResultURL,
        ResponseType
      };

      dispatch({ type: SUBMIT_CARD + FETCH });

      axios({
        method: 'post',
        url: URL3DSi,
        headers: { 'Content-type': 'application/x-www-form-urlencoded' },
        data
      });

      redirectUser(dispatch, RedirectURL, submissionId);

    } catch (e) {
      dispatch({ type: SUBMIT_CARD + FAILURE });
    }
  };
};
