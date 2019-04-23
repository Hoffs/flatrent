import React from "react";
import ReactDOM from "react-dom";
import App from "./App";
import "./index.css";
import * as serviceWorker from "./serviceWorker";
import "./variables.css";
import Moment from "moment";

// // tslint:disable-next-line: no-submodule-imports
// import DefaultTheme from "react-dates/lib/theme/DefaultTheme";
// // tslint:disable-next-line: no-submodule-imports
// import aphroditeInterface from "react-with-styles-interface-aphrodite";
// // tslint:disable-next-line: no-submodule-imports
// import ThemedStyleSheet from "react-with-styles/lib/ThemedStyleSheet";

// ThemedStyleSheet.registerInterface(aphroditeInterface);
// ThemedStyleSheet.registerTheme(DefaultTheme);

Moment.locale("lt");

ReactDOM.render(<App />, document.getElementById("root"));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: http://bit.ly/CRA-PWA
serviceWorker.unregister();
