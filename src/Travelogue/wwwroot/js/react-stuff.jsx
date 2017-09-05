var data = [
  { id: 1, author: "Daniel Lo Nigro", text: "Hello ReactJS.NET World!" },
  { id: 2, author: "Pete Hunt", text: "This is one comment" },
  { id: 3, author: "Jordan Walke", text: "This is *another* comment" }
];


var CommentList = React.createClass({
  render: function() {
    var commentNodes = this.props.data.map(function(comment) {
      return (
        <Comment author={comment.author} key={comment.id}>{comment.text}
        </Comment>
      );
    });
    return (
      <div className="commentList">{commentNodes}
      </div>
    );
  }
});


var CommentForm = React.createClass({
    getInitialState: function () {
        return { data: { comments: [] } };
    },
    render: function() {
        return (
          <div className="commentForm">
            Hello, world! I am a CommentForm.
          </div>
      );
    }
});
var CommentBox = React.createClass({
  render: function() {
    return (
      <div className="commentBox">
        <h1>Comments</h1>
        <CommentList data={this.props.data} />
        <CommentForm />
      </div>
    );
  }
});
var Comment = React.createClass({
  rawMarkup: function() {
    var md = new Remarkable();
    var rawMarkup = md.render(this.props.children.toString());
    return { __html: rawMarkup };
  },

  render: function() {
    return (
      <div className="comment">
        <h2 className="commentAuthor">{this.props.author}
        </h2>
        <span dangerouslySetInnerHTML={this.rawMarkup()} />
      </div>
    );
  }
});

ReactDOM.render(
  <CommentBox url="/comments" />,
  document.getElementById('content')
);


//var CommentList = React.createClass({
//    render: function() {
//        var commentNodes = this.props.data.map(function (comment){
//            return (
//              <div>
//                <h1>{comment.author}</h1>
//              </div>
//            );
//        });
//        return (
//          <div className="commentList">{commentNodes}
//          </div>
//      );
//    }
//});
//var CommentBox = React.createClass({
//    getInitialState: function(){
//        return {data: {comments:[]}};
//    },
//    getComments: function(){
//        // mock ajax operation
//        var success = function(){
//            var data = {
//                comments : [
//                    { author : 'Mark Twein' },
//                    { author : 'Ernest Hemingway' },
//                    { author : 'Lewis Carroll' }
//                ]
//            };
//            this.setState( {data: data} );
//        }.bind(this);
//        setTimeout(success, 100);
//        /*
//          $.ajax({
//            url: this.props.url,
//            dataType: 'json',
//            success: function(data){
//              this.setState({data: data});
//            }.bind(this),
//            error: function(xhr, status, err){
//              console.error(url, status, err.toString());
//            }.bind(this)
//          });*/
//    },
//    componentWillMount: function(){
//        this.getComments();
//    },
//    render: function() {
//        return (
//          <div className="commentBox">{/*this.state.data.comments*/}
//        {<CommentList data={this.state.data.comments} />}
//        </div>
//      );
//    }
//});
