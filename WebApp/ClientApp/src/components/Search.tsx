import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as SearchStore from '../store/SimpleSearch';
import './Search.css'
import loader from '../assets/ajax-loader.gif';
import googleIcon from '../assets/google-2015.svg';
import ddgIcon from '../assets/duckduckgo.svg';
import bingIcon from '../assets/bing-11.svg';

type CounterProps =
    SearchStore.SearchState &
    typeof SearchStore.actionCreators &
    RouteComponentProps<{}>;

class SearchCounter extends React.PureComponent<CounterProps, {searchBoxValue: string}> {
    constructor(props: Readonly<CounterProps>) {
       super(props);

       this.state = {
           searchBoxValue: ''
       };

       this.updateSearchTerm = this.updateSearchTerm.bind(this);
       this.doSearch = this.doSearch.bind(this);
    }

    public render() {
        return (
            <React.Fragment>
                <div className="searchContainer">                
                    <h1 id="tabelLabel">Search</h1>    
                    <input id="searchBox" onChange={(event) => this.updateSearchTerm(event)}></input>
                    <button className='btn btn-outline-secondary searchButton' onClick={() => this.doSearch()}>Go</button>
                    {this.props.isLoading && <div className='loaderDiv'><span>Loading...</span> <img src={loader} alt='loader'></img></div> }
                </div>

                <div className="resultsContainer">
                    
                    {this.props.searchResults && this.props.searchResults.map((result: SearchStore.SearchResult) =>
                        <div className="resultDiv" key={result.url}>
                            <a href={result.url}>{result.label}</a>
                            <span>{this.renderProviders(result.searchEngine)}</span>
                        </div>
                    )}
                </div>
            </React.Fragment>
        );
    }

    private updateSearchTerm(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState(
           {
               searchBoxValue: event.target.value
           }
        );
    }    

    private doSearch() {
        this.props.requestSearchResults(this.state.searchBoxValue);
    }    

    private renderProviders(providers: SearchStore.SearchProvider[]){
        return <div>
        {providers && providers.map((provider: SearchStore.SearchProvider) =>
            <span>{this.getProviderName(provider)}</span>
        )}            
            </div>
    }


    private getProviderName(provider: SearchStore.SearchProvider){
        switch (provider) {
            case SearchStore.SearchProvider.Google:
                return <img src={googleIcon} alt="google"></img>;
            case SearchStore.SearchProvider.Bing:
                return <img src={bingIcon} alt="bing"></img>;
            case SearchStore.SearchProvider.DuckDuckGo:
                return <img src={ddgIcon} alt="duckduckgo"></img>;                          
        }

        return "";
    }
};

export default connect(
    (state: ApplicationState) => state.simpleSearch,
    SearchStore.actionCreators
)(SearchCounter as any);
