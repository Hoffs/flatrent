import React, { Component } from "react";
import SVG from "react-inlinesvg";
import { RouteComponentProps, withRouter, Link } from "react-router-dom";
import logo from "../../logo.svg";
import { Routes } from "../../Routes";
import UserService from "../../services/UserService";
import LinkWithHighlight from "./LinkWithHighlight";
import NavigationButton from "./NavigationButton";
import Styles from "./NavBar.module.css";

class NavBar extends Component<RouteComponentProps> {
  constructor(props: Readonly<RouteComponentProps>) {
    super(props);
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

  private getLinks() {
    const isLoggedIn = UserService.isLoggedIn();
    const filteredLinks = Routes.filter(
      (link) =>
        link.addToNav &&
        (link.authenticated === isLoggedIn || link.authenticated === undefined) &&
        UserService.hasRoles(...link.roles)
    );
    return filteredLinks.map((link, index) => (
      <LinkWithHighlight link={link.link} currentUrl={this.props.location.pathname} key={index}>
        {link.text}
      </LinkWithHighlight>
    ));
  }

  private getNavigationButtons() {
    const isLoggedIn = UserService.isLoggedIn();
    const filteredLinks = Routes.filter(
      (link) =>
        link.addToNav &&
        (link.authenticated === isLoggedIn || link.authenticated === undefined) &&
        UserService.hasRoles(...link.roles)
    );
    return filteredLinks.map((link, index) => (
      <NavigationButton link={link.link} currentUrl={this.props.location.pathname} key={index}>
        {link.text}
      </NavigationButton>
    ));
  }
}

export default withRouter(NavBar);
