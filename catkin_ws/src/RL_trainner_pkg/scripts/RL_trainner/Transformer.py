import torch
import torch.nn as nn
class Self_Attention(nn.Module):
    """Multi Head Self Attention  + Dense Layer.
    Parameters
    ----------
    dim : int
        The input and out dimension of per token features.
    n_heads : int
        Number of attention heads.
    qkv_bias : bool
        If True then we include bias to the query, key and value projections.
    attn_p : float
        Dropout probability applied to the query, key and value tensors.
    proj_p : float
        Dropout probability applied to the output tensor.
    Attributes
    ----------
    scale : float
        Normalizing consant for the dot product.
    W_q : nn.Linear
        Linear projection for the query
    W_k : nn.Linear
        Linear projection for the keywords
    W_v : nn.Linear
        Linear projection for the value

    proj : nn.Linear
        Linear mapping that takes in the concatenated output of all attention
        heads and maps it into a new space.
    attn_drop, proj_drop : nn.Dropout
        Dropout layers.
    """
    def __init__(self, dim, n_heads, qkv_bias=True, attn_p=0., proj_p=0.):
        super().__init__()
        self.dim = dim
        self.n_heads = n_heads
        self.dim_oneHead = dim//n_heads
        self.scale = self.dim_oneHead**-0.5
        #Layers
        self.W_q = nn.Linear(in_features=dim, out_features=dim, bias=qkv_bias)
        self.W_k = nn.Linear(in_features=dim, out_features=dim, bias=qkv_bias)
        self.W_v = nn.Linear(in_features=dim, out_features=dim, bias=qkv_bias)
        self.attn_drop = nn.Dropout(attn_p)
        self.proj_drop = nn.Dropout(proj_p)
        self.proj = nn.Sequential(
            nn.Linear(in_features=dim, out_features=dim),
            nn.ReLU()
        )

    def forward(self,x):
        """Run forward pass.
        Parameters
        ----------
        x : torch.Tensor
            Shape `(n_samples, n_patches + 1, dim)`.
        Returns
        -------
        torch.Tensor
            Shape `(n_samples, n_patches + 1, dim)`.
        """
        n_samples, n_tokens, dim = x.shape#n_tokens = n_patches+1

        if dim!= self.dim:
            raise ValueError

        Q = self.W_q(x)#shape:(n_samples, n_patches + 1, dim)
        K = self.W_k(x)#shape:(n_samples, n_patches + 1, dim)
        V = self.W_v(x)#shape:(n_samples, n_patches + 1, dim)

        Q = Q.reshape(n_samples, n_tokens, self.n_heads, self.dim_oneHead) # shape: (n_samples, n_patches+1, n_heads, dim_oneHead)
        K = K.reshape(n_samples, n_tokens, self.n_heads, self.dim_oneHead) # shape: (n_samples, n_patches+1, n_heads, dim_oneHead)
        V = V.reshape(n_samples, n_tokens, self.n_heads, self.dim_oneHead) # shape: (n_samples, n_patches+1, n_heads, dim_oneHead)

        Q = Q.permute(0, 2, 1, 3)  # shape:(n_samples, n_heads, n_patches+1, dim_oneHead)
        K = K.permute(0, 2, 1, 3)  # shape:(n_samples, n_heads, n_patches+1, dim_oneHead)
        V = V.permute(0, 2, 1, 3)  # shape:(n_samples, n_heads, n_patches+1, dim_oneHead)
        #further permutation to aline with the formular in the paper

        Q = Q.permute(0, 1, 3, 2)  # shape:(n_samples, n_heads, dim_oneHead, n_patches+1)
        K = K.permute(0, 1, 3, 2)  # shape:(n_samples, n_heads, dim_oneHead, n_patches+1)
        V = V.permute(0, 1, 3, 2)  # shape:(n_samples, n_heads, dim_oneHead, n_patches+1)

        K_T = K.transpose(-2,-1) # shape:(n_samples, n_heads, n_patches+1, dim_oneHead)

        #Calculate Attention Weight
        W_atten = ((K_T @ Q) * self.scale).softmax(dim = -2)# shape:(n_samples, n_heads, n_patches+1, n_patches+1)
        W_atten = self.attn_drop(W_atten)

        #Calculate the context feature
        C = V @ W_atten # shape:(n_samples, n_heads, dim_oneHead, n_patches+1)

        #A dense Layer
        #  permutation back
        C = C.permute(0, 1, 3, 2) # shape:(n_samples, n_heads, n_patches+1, dim_oneHead)

        C = C.transpose(1,2) # shape:(n_samples, n_patches+1, n_heads, dim_oneHead)
        C = C.flatten(2)# shape:(n_samples, n_patches+1, dim) ; n_heads * dim_oneHead = dim
        x = self.proj(C)# shape:(n_samples, n_patches+1, dim)
        x = self.proj_drop(x)# shape:(n_samples, n_patches+1, dim)

        return x
    
class MLP(nn.Module):
    """Multilayer perceptron.
    Parameters
    ----------
    in_features : int
        Number of input features.
    hidden_features : int
        Number of nodes in the hidden layer.
    out_features : int
        Number of output features.
    p : float
        Dropout probability.
    Attributes
    ----------
    fc : nn.Linear
        The First linear layer.
    act : nn.GELU
        GELU activation function.
    fc2 : nn.Linear
        The second linear layer.
    drop : nn.Dropout
        Dropout layer.
    """
    def __init__(self, in_features, hidden_features, out_features, p=0):
        super().__init__()
        self.fc1 = nn.Linear(in_features=in_features, out_features=hidden_features)
        self.act = nn.GELU()
        self.fc2 = nn.Linear(in_features=hidden_features, out_features=out_features)
        self.drop = nn.Dropout(p)

    def forward(self,x):
        """Run forward pass.
        Parameters
        ----------
        x : torch.Tensor
            Shape `(n_samples, n_patches + 1, in_features)`.
        Returns
        -------
        torch.Tensor
            Shape `(n_samples, n_patches +1, out_features)`
        """
        x = self.fc1(x)# (n_samples, n_patches + 1, hidden_features)
        x = self.act(x)# (n_samples, n_patches + 1, hidden_features)
        x = self.drop(x)# (n_samples, n_patches + 1, hidden_features)
        x = self.fc2(x)# (n_samples, n_patches + 1, out_features)
        x = self.drop(x)# (n_samples, n_patches + 1, out_features)

        return x
    
class Block(nn.Module):
    """Transformer block.
    Parameters
    ----------
    dim : int
        Embeddinig dimension.
    n_heads : int
        Number of attention heads.
    mlp_ratio : float
        Determines the hidden dimension size of the `MLP` module with respect
        to `dim`.
    qkv_bias : bool
        If True then we include bias to the query, key and value projections.
    p, attn_p : float
        Dropout probability.
    Attributes
    ----------
    norm1, norm2 : LayerNorm
        Layer normalization.
    attn : Attention
        Attention module.
    mlp : MLP
        MLP module.
    """
    def __init__(self, dim, n_heads, mlp_ratio=4.0, qkv_bias=True, p=0., attn_p=0,):
        super().__init__()
        mlp_ratio=4.0
        hidden_features = int(dim*mlp_ratio)

        self.norm1 = nn.LayerNorm(dim, eps=1e-6)
        self.attn = Self_Attention(
            dim,
            n_heads=n_heads,
            qkv_bias=qkv_bias,
            attn_p=attn_p,
            proj_p=p
        )
        self.norm2 = nn.LayerNorm(dim, eps=1e-6)
        self.mlp = MLP(
            in_features=dim,
            hidden_features=hidden_features,
            out_features=dim
        )
    def forward(self, x):
        """Run forward pass.
        Parameters
        ----------
        x : torch.Tensor
            Shape `(n_samples, n_patches + 1, dim)`.
        Returns
        -------
        torch.Tensor
            Shape `(n_samples, n_patches + 1, dim)`.
        """
        x = x + self.attn(self.norm1(x))
        x = x + self.mlp(self.norm2(x))

        return x
# class TransformerModel(BaseModel):
#     """
#     This is a dummy model. It provides basic implementations to demonstrate how more advanced models can be built.
#     """

#     def __init__(self, config):
#         self.n_history = 20
#         super(TransformerModel, self).__init__(config)

#     # noinspection PyAttributeOutsideInit
#     def create_model(self):
#         # In this model we simply feed the last time steps of the seed to a dense layer and
#         # predict the targets directly.
#         # self.dense = nn.Linear(in_features=self.n_history * self.pose_size,
#         #                        out_features=self.config.target_seq_len * self.pose_size)

#         # Transformer
#         self.depth = 5
#         self.n_heads = 5
#         self.entity_dim = self.pose_size
#         self.num_tokens = self.n_history
#         self.out_dim = self.config.target_seq_len * self.pose_size


#         self.cls_token = nn.Parameter(torch.zeros(1, 1, self.entity_dim))
#         self.pos_embed = nn.Parameter(
#                 torch.zeros(1, self.num_tokens, self.entity_dim)
#         )#Dim(1,T,D)

#         self.blocks = nn.ModuleList()
#         for _ in range(self.depth):
#             self.blocks.append(Block(dim = self.entity_dim , n_heads=self.n_heads))

#         self.norm = nn.LayerNorm(self.entity_dim, eps=1e-6)
#         self.head = nn.Linear(self.entity_dim, self.out_dim)

#     def forward(self, batch: AMASSBatch):
#         """
#         The forward pass.
#         :param batch: Current batch of data.
#         :return: Each forward pass must return a dictionary with keys {'seed', 'predictions'}.
#         """
#         model_out = {'seed': batch.poses[:, :self.config.seed_seq_len],
#                      'predictions': None}
#         batch_size = batch.batch_size
#         model_in = batch.poses[:, (self.config.seed_seq_len-self.n_history):self.config.seed_seq_len]#Dim(batch_size,T,D)=(16,10,135)


#         #---Main Model---
#         x = model_in  #Dim(batch_size,T,D) = (n_samples , num_tokens, entity_dim)
#         barch_size = x.shape[0]# (2048)
#         x = x  + self.pos_embed
#         for block in self.blocks:
#             x = block(x)
#         #Dim(batch_size, num_tokens, entity_dim)     
#         x = self.norm(x)#Dim(batch_size, num_tokens,entity_dim)
#         cls_token_final = x[:, 0]  # just the CLS token ;Dim(batch_size,entity_dim)
#         x = self.head(cls_token_final)#Dim(batch_size,self.config.target_seq_len * self.pose_size) = (batch_size ,out_dim)
#         #---------------- 
#         pred=x
#         model_out['predictions'] = pred.reshape(batch_size, self.config.target_seq_len, -1)
#         return model_out

#     def backward(self, batch: AMASSBatch, model_out):
#         """
#         The backward pass.
#         :param batch: The same batch of data that was passed into the forward pass.
#         :param model_out: Whatever the forward pass returned.
#         :return: The loss values for book-keeping, as well as the targets for convenience.
#         """
#         predictions = model_out['predictions']
#         targets = batch.poses[:, self.config.seed_seq_len:]

#         total_loss = mse(predictions, targets)

#         # If you have more than just one loss, just add them to this dict and they will automatically be logged.
#         loss_vals = {'total_loss': total_loss.cpu().item()}

#         if self.training:
#             # We only want to do backpropagation in training mode, as this function might also be called when evaluating
#             # the model on the validation set.
#             total_loss.backward()

#         return loss_vals, targets