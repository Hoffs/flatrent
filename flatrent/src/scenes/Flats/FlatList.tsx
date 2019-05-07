import React, { Component, ReactNode } from "react";
import InfiniteScroll from "react-infinite-scroller";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import FlexRow from "../../components/FlexRow";
import FlatService from "../../services/FlatService";
import { IApiResponse } from "../../services/interfaces/Common";
import { IShortFlatDetails } from "../../services/interfaces/FlatServiceInterfaces";
import FlatBox, { FlatBoxLoader } from "./FlatBox";
import Styles from "./FlatList.module.css";
import FilterList from "./components/FilterList";
import FlexColumn from "../../components/FlexColumn";

class FlatList extends Component<
    RouteComponentProps,
    { pageSize: number; page: number; hasMore: boolean; flats: IShortFlatDetails[], filters: string }
> {
    constructor(props: Readonly<RouteComponentProps>) {
        super(props);
        this.state = { pageSize: 20, page: 0, hasMore: true, flats: [], filters: props.location.search.replace("?", "") };
        this.loadFlats(this.state.page);
    }

    public render() {
        return (
            <FlexColumn>
                <FilterList className={Styles.filters} onFilterUpdate={this.updateFilters} {...this.props} />
                <FlexRow className={Styles.flatBoxes}>
                    <InfiniteScroll
                        className={Styles.scroller}
                        pageStart={0}
                        initialLoad={false}
                        loadMore={this.loadFlats}
                        hasMore={this.state.hasMore}
                        loader={this.getLoaderItems(0)}
                        useWindow={true}
                        key={0}
                    >
                        {this.getFlatItems()}
                    </InfiniteScroll>
                </FlexRow>
            </FlexColumn>
        );
    }

    private getFlatItems(): ReactNode[] {
        const flats = this.state.flats.map((flat) => <FlatBox key={flat.id} flat={flat} />);
        if (flats.length > 0) {
            return flats;
        } else if (this.state.hasMore) {
            return Array(12)
                .fill(0)
                .map((_, idx) => <FlatBoxLoader key={idx} />);
        } else {
            return [<></>];
        }
    }

    private getLoaderItems(count: number) {
        const els = Array(count)
            .fill(0)
            .map((_, idx) => <FlatBoxLoader key={idx + 50} />);
        return <>{els}</>;
    }

    private loadFlats = (_: number) => {
        FlatService.getFlats(this.state.pageSize, this.state.filters, this.state.flats.length)
            .then(this.handleFlatResult)
            .catch(this.handleFail);
    };

    private handleFlatResult = (result: IApiResponse<IShortFlatDetails[]>) => {
        if (result.errors !== undefined) {
            const errors = Object.keys(result.errors).map((key) => result.errors![key].join("\n"));
            errors.forEach((error) => toast.error(error));
        } else if (result.data !== undefined) {
            this.setState((state) => ({
                flats: [...state.flats, ...result.data!],
                hasMore: result.data!.length !== 0,
            }));
        }
    };

    private updateFilters = (query: string) => {
        this.setState({ flats: [], hasMore: true, filters: query }, () => this.loadFlats(0));
    };

    private handleFail() {
        toast.error("Įvyko nežinoma klaida.");
    }
}

export default FlatList;
