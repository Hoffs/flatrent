import React, { Component } from "react";
import SVG from "react-inlinesvg";
import { RouteComponentProps, withRouter, Link } from "react-router-dom";
import logo from "../../logo.svg";
import { getApplicableRoutes } from "../../Routes";
import UserService from "../../services/UserService";
import LinkWithHighlight from "./LinkWithHighlight";
import NavigationButton from "./NavigationButton";
import Styles from "./NavBar.module.css";
import { fLocalStorage } from "../../utilities/LocalStorageWrapper";

class NavBar extends Component<RouteComponentProps> {
  private listenerId: string;

  constructor(props: Readonly<RouteComponentProps>) {
    super(props);
    this.listenerId = fLocalStorage.addListener(this.onStorageEvent);
  }

  public componentWillUnmount() {
    fLocalStorage.removeListener(this.listenerId);
  }

  public render() {
    return (
      <div className={Styles.navbar}>
        <Link className={Styles.logoLink} to="/">
          <div className={Styles.logo}>
            <SVG className={Styles.svg} src={logo} />
          </div>
        </Link>
        {/* <nav className={Styles.nav}>
          <ul className={Styles.ul}>
            {this.getLinks()}
          </ul>
        </nav> */}
        <div className={Styles.nav} >
          {this.getNavigationButtons()}
        </div>
      </div>
    );
  }

  private onStorageEvent = (key: string, value: string, action: string) => {
    console.log("storageEvent received");
    this.forceUpdate();
  }

  private getLinks() {
    const filteredLinks = getApplicableRoutes().filter((link) => link.addToNav);
    return filteredLinks.map((link, index) => (
      <LinkWithHighlight link={link.link} currentUrl={this.props.location.pathname} key={index}>
        {link.text}
      </LinkWithHighlight>
    ));
  }

  private getNavigationButtons() {
    const filteredLinks = getApplicableRoutes().filter((link) => link.addToNav);
    return filteredLinks.map((link, index) => (
      <NavigationButton link={link.link} currentUrl={this.props.location.pathname} key={index}>
        {link.text}
      </NavigationButton>
    ));
  }
}

export default withRouter(NavBar);
